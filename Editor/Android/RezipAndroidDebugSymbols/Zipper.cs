using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UniGame.ZipTool
{
    using Unity.SharpZipLib.Checksum;
    using Unity.SharpZipLib.Zip;
    using Unity.SharpZipLib.Zip.Compression;

    public static class Zipper
    {
        public const string ToolName = "7z";
        public const string Category = "Tools";
        public const string ZipCommandFormat = "a -tzip \"{0}\"";
        public const string UnZipCommandFormat = "x \"{1}\" -o\"{0}\" ";
        public const string WinExtension = ".exe";

        public static void RunUnzip(string location, string intermediatePath, string targetDirectory)
        {
            var zipFileName = GetZipTool();

            FileCommand.Cleanup(intermediatePath);

            var args = string.Format(UnZipCommandFormat, intermediatePath, location);
            var result = ProcessRunner.RunProcess(targetDirectory, zipFileName, args);
            if (result.Failure)
                throw new Exception(result.ToString());
        }

        public static string GetZipTool()
        {
            var zipFileName =
                Path.GetFullPath(Path.Combine(EditorApplication.applicationContentsPath, Category, ToolName));

            if (Application.platform == RuntimePlatform.WindowsEditor)
                zipFileName += WinExtension;

            if (!File.Exists(zipFileName))
                throw new Exception($"Failed to locate {zipFileName}");

            return zipFileName;
        }

        public static ProcessResult RunZip(string location, string zipResultName)
        {
            var zipFileName = GetZipTool();
            var args = string.Format(ZipCommandFormat, zipResultName);
            return ProcessRunner.RunProcess(location, zipFileName, args);
        }
    }


    /// <summary>
    /// Uses Sharpziplib so as to create a non flat zip archive
    /// </summary>
    public class ZipWorker
    {
        public const string ZipFormat = ".zip";

        public static void Unzip(string pathToZip, string targetLocation,string fileFilter = null)
        {
            if (File.Exists(pathToZip) == false)
            {
                Debug.LogError($"ZIP FILE NOT FOUND {pathToZip}");
                return;
            }
            
            var fastZip = new FastZip();

            // Will always overwrite if target filenames already exist
            fastZip.ExtractZip(pathToZip, targetLocation, fileFilter);
        }
        
        /// <summary>
        /// will zip directory .\toto as .\toto.zip
        /// </summary>
        /// <param name="stDirToZip"></param>
        /// <returns></returns>
        public static string CreateZip(string stDirToZip)
        {
            var result = string.Empty;
            
            try
            {
                var di = new DirectoryInfo(stDirToZip);
                var parent = di.Parent == null
                    ? string.Empty
                    : string.IsNullOrEmpty(di.Parent.FullName)
                        ? string.Empty
                        : di.Parent.FullName;
                
                var stZipPath = Path.Combine(parent , di.Name + ZipFormat);
                CreateZip(stZipPath, stDirToZip);
                result = stZipPath;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return result;
        }

        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="resultZipFile">path of the archive wanted</param>
        /// <param name="sourceDirectory">path of the directory we want to create, without ending backslash</param>
        /// <param name="overrideFile"></param>
        /// <param name="compressionLevel"></param>
        /// <param name="recursive"></param>
        /// <param name="filter"></param>
        public static void CreateZip(string resultZipFile, string sourceDirectory,bool recursive = true, Deflater.CompressionLevel compressionLevel = Deflater.CompressionLevel.DEFLATED,string filter = null)
        {
            var fastZip = new FastZip();
            
            fastZip.CreateZip(resultZipFile,sourceDirectory,recursive,filter);
            try
            {
                //Sanitize inputs
                sourceDirectory = Path.GetFullPath(sourceDirectory);
                resultZipFile = Path.GetFullPath(resultZipFile);

                Console.WriteLine("Zip directory " + sourceDirectory);

                //Recursively parse the directory to zip 
                var stackFiles = DirExplore(sourceDirectory);

                ZipOutputStream zipOutput = null;

                var fileExists = File.Exists(resultZipFile);
                if (fileExists)
                    File.Delete(resultZipFile);

                var crc = new Crc32();
                using (zipOutput = new ZipOutputStream(File.Create(resultZipFile)))
                {
                
                    zipOutput.SetLevel(6); // 0 - store only to 9 - means best compression

                    Console.WriteLine(stackFiles.Count + " files to zip.\n");

                    var index = 0;
                    foreach (var fi in stackFiles)
                    {
                        ++index;
                        var percent = (int) ((float) index / ((float) stackFiles.Count / 100));
                        if (percent % 1 == 0)
                        {
                            Console.CursorLeft = 0;
                            Console.Write(_stSchon[index % _stSchon.Length].ToString() + " " + percent + "% done.");
                        }

                        var fs = File.OpenRead(fi.FullName);

                        var buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);

                        //Create the right arborescence within the archive
                        var stFileName = fi.FullName.Remove(0, sourceDirectory.Length + 1);
                        var entry = new ZipEntry(stFileName) {DateTime = DateTime.Now, Size = fs.Length};

                        // set Size and the crc, because the information
                        // about the size and crc should be stored in the header
                        // if it is not set it is automatically written in the footer.
                        // (in this case size == crc == -1 in the header)
                        // Some ZIP programs have problems with zip files that don't store
                        // the size and crc in the header.
                        fs.Close();
                        crc.Reset();
                        crc.Update(buffer);

                        entry.Crc = crc.Value;

                        zipOutput.PutNextEntry(entry);

                        zipOutput.Write(buffer, 0, buffer.Length);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        private static Stack<FileInfo> DirExplore(string stSrcDirPath)
        {
            try
            {
                var stackDirs = new Stack<DirectoryInfo>();
                var stackPaths = new Stack<FileInfo>();

                var dd = new DirectoryInfo(Path.GetFullPath(stSrcDirPath));

                stackDirs.Push(dd);
                while (stackDirs.Count > 0)
                {
                    var currentDir = (DirectoryInfo) stackDirs.Pop();

                    try
                    {
                        //Process .\files
                        foreach (var fileInfo in currentDir.GetFiles())
                        {
                            stackPaths.Push(fileInfo);
                        }

                        //Process Subdirectories
                        foreach (var diNext in currentDir.GetDirectories())
                            stackDirs.Push(diNext);
                    }
                    catch (Exception)
                    {
                        //Might be a system directory
                    }
                }

                return stackPaths;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static char[] _stSchon = new char[] {'-', '\\', '|', '/'};
    }
}