using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using UniGame.ZipTool;
using UniModules;
using UniGame.Runtime.Extension;
using UniGame.Runtime.Utils;

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
        FileCommand.Cleanup(intermediatePath);
        ZipWorker.Unzip(location, intermediatePath);
        
        EditorUtility.DisplayProgressBar("Shrinking symbols", "Deleting/Renaming/Compressing symbol files", 0.5f);

        //remove unsupported AndroidArchitecture
        RemoveUnsupportedAndroidArchitecture(intermediatePath,supportedAndroidArchitecture);
        
        FileCommand.Cleanup(newZip);

        CleanUpDebugFiles(intermediatePath);

        ZipWorker.CreateZip(newZip, intermediatePath);

        EditorUtility.ClearProgressBar();

        FileCommand.Cleanup(intermediatePath);

        Debug.Log($"New small symbol package: {newZip}");

        return true;
    }

    public static void CleanUpDebugFiles(string location)
    {
        var files = Directory.GetFiles(location, AllFiles, SearchOption.AllDirectories);

        foreach (var file in files)
        {
            if (removedFiles.Any(x => file.EndsWith(x)))
            {
                FileCommand.Cleanup(file);
                continue;
            }

            if (!file.EndsWith(DebugSymbolsExtension))
            {
                continue;
            }

            var fileSo = string.Format(DebugSymbolFileTemplate, file.Substring(0, file.Length - DebugSymbolsExtension.Length));
            
            Debug.Log($"Rename {file} --> {fileSo}");
            
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

    
    
}