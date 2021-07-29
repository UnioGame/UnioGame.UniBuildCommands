using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UniModules.UniCore.Runtime.Extension;
using UniModules.UniCore.Runtime.Utils;
using UniModules.UniGame.Core.EditorTools.Editor.Tools;

public class AndroidSymbolShrinker
{
    public const string DebugSymbolsExtension       = ".sym.so";
    public const string ZipFormat                   = "*.zip";
    public const string NewDebugSymbolsNameTemplate = "{0}.small.zip";
    public const string IntermediatePathName        = "TempShrink";
    public const string AllFiles                    = "*.*";
    public const string DebugSymbolFileTemplate     = "{0}.so";

    private static string LastSymbolToShrinkLocation = nameof(LastSymbolToShrinkLocation);

    private static List<string> removedFiles = new List<string>()
    {
        ".dbg.so"
    };

    private static Dictionary<AndroidArchitecture, string> androidArchitectereLocations = new Dictionary<AndroidArchitecture, string>()
    {
        {AndroidArchitecture.ARM64, "arm64-v8a"},
        {AndroidArchitecture.ARMv7, "armeabi-v7a"},
    };

    private static void Log(string message)
    {
        UnityEngine.Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, message);
    }

    private static void Cleanup(string path)
    {
        if (Directory.Exists(path))
        {
            Log($"Delete {path}");
            Directory.Delete(path, true);
        }

        if (File.Exists(path))
        {
            Log($"Delete {path}");
            File.Delete(path);
        }
    }

    [MenuItem("Android Symbols/Shrink")]
    public static void ShrinkSymbols()
    {
        var location = EditorPrefs.GetString(LastSymbolToShrinkLocation, Path.Combine(Application.dataPath, ".."));
        location = EditorUtility.OpenFilePanel("Open Android Symbol Package to shrink", location, ZipFormat);

        ShrinkSymbols(location);
    }

    public static bool ShrinkSymbols(string location, AndroidArchitecture supportedAndroidArchitecture = AndroidArchitecture.All)
    {
        if (string.IsNullOrEmpty(location) || File.Exists(location) == false)
            return false;

        var targetDirectory  = Path.GetDirectoryName(location);
        var intermediatePath = Path.Combine(targetDirectory, IntermediatePathName);
        var newZip           = Path.Combine(targetDirectory,string.Format(NewDebugSymbolsNameTemplate,Path.GetFileNameWithoutExtension(location)));

        EditorPrefs.SetString(LastSymbolToShrinkLocation, targetDirectory);

        Zipper.RunUnzip(location, intermediatePath, targetDirectory);

        EditorUtility.DisplayProgressBar("Shrinking symbols", "Deleting/Renaming/Compressing symbol files", 0.5f);

        //remove unsupported AndroidArchitecture
        RemoveUnsupportedAndroidArchitecture(intermediatePath,supportedAndroidArchitecture);
        
        Cleanup(newZip);

        CleanUpDebugFiles(intermediatePath);

        var result = Zipper.RunZip(intermediatePath, newZip);

        EditorUtility.ClearProgressBar();
        if (result.Failure)
            throw new Exception(result.ToString());

        Cleanup(intermediatePath);

        Log($"New small symbol package: {newZip}");

        EditorUtility.RevealInFinder(newZip);

        return true;
    }

    public static void CleanUpDebugFiles(string location)
    {
        var files = Directory.GetFiles(location, AllFiles, SearchOption.AllDirectories);

        foreach (var file in files)
        {
            if (removedFiles.Any(x => file.EndsWith(x)))
            {
                Cleanup(file);
                continue;
            }

            if (!file.EndsWith(DebugSymbolsExtension))
            {
                continue;
            }

            var fileSo = string.Format(DebugSymbolFileTemplate, file.Substring(0, file.Length - DebugSymbolsExtension.Length));
            
            Log($"Rename {file} --> {fileSo}");
            
            File.Move(file, fileSo);
        }
    }

    private static void RemoveUnsupportedAndroidArchitecture(string location, AndroidArchitecture supportedAndroidArchitecture)
    {
        var values = EnumValue<AndroidArchitecture>.Values;
        foreach (var value in values)
        {
            if (supportedAndroidArchitecture.IsFlagSet(value))
                continue;

            if(!androidArchitectereLocations.TryGetValue(value,out var directoryName))
                continue;
            
            var path          = location.CombinePath(directoryName);
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }
    }

    public static class Zipper
    {
        public const string ToolName           = "7z";
        public const string Category           = "Tools";
        public const string ZipCommandFormat   = "a -tzip \"{0}\"";
        public const string UnZipCommandFormat = "x \"{1}\" -o\"{0}\" ";
        public const string WinExtension       = ".exe";

        public static void RunUnzip(string location, string intermediatePath, string targetDirectory)
        {
            var zipFileName = GetZipTool();

            Cleanup(intermediatePath);

            var args   = string.Format(UnZipCommandFormat, intermediatePath, location);
            var result = ProcessRunner.RunProcess(targetDirectory, zipFileName, args);
            if (result.Failure)
                throw new Exception(result.ToString());
        }

        public static string GetZipTool()
        {
            var zipFileName = Path.GetFullPath(Path.Combine(EditorApplication.applicationContentsPath, Category, ToolName));

            if (Application.platform == RuntimePlatform.WindowsEditor)
                zipFileName += WinExtension;

            if (!File.Exists(zipFileName))
                throw new Exception($"Failed to locate {zipFileName}");

            return zipFileName;
        }

        public static ProcessResult RunZip(string location, string zipResultName)
        {
            var zipFileName = GetZipTool();
            var args        = string.Format(ZipCommandFormat, zipResultName);
            return ProcessRunner.RunProcess(location, zipFileName, args);
        }
    }

    public class ProcessResult
    {
        public int    ExitCode { get; }
        public string StdOut   { get; }
        public string StdErr   { get; }

        internal bool Failure => ExitCode != 0;

        internal ProcessResult(int exitCode, string stdOut, string stdErr)
        {
            ExitCode = exitCode;
            StdOut   = stdOut;
            StdErr   = stdErr;
        }

        public override string ToString()
        {
            return $"Exit Code: {ExitCode}\nStdOut:\n{StdOut}\nStdErr:\n{StdErr}";
        }
    }

    public static class ProcessRunner
    {
        public static ProcessResult RunProcess(string workingDirectory, string fileName, string args)
        {
            Log($"Executing {fileName} {args} (Working Directory: {workingDirectory}");
            var process = new Process();
            process.StartInfo.FileName               = fileName;
            process.StartInfo.Arguments              = args;
            process.StartInfo.UseShellExecute        = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError  = true;
            process.StartInfo.WorkingDirectory       = workingDirectory;
            process.StartInfo.CreateNoWindow         = true;
            var output = new StringBuilder();

            process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    output.AppendLine(e.Data);
                }
            });

            var error = new StringBuilder();
            process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    error.AppendLine(e.Data);
                }
            });

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            Log($"{fileName} exited with {process.ExitCode}");
            return new ProcessResult(process.ExitCode, output.ToString(), error.ToString());
        }
    }
}