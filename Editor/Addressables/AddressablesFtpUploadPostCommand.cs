namespace UniGame.BuildCommands.Editor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using AddressableTools.Editor;
    using FluentFTP;
    using global::UniGame.UniBuild.Editor;
    using UniModules;
    using UnityEngine;
    using UnityEngine.Scripting.APIUpdating;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    [Serializable]
    [MovedFrom(sourceNamespace:"UniModules.UniGame.BuildCommands.Editor.Ftp")]
    public class AddressablesFtpUploadPostCommand : SerializableBuildCommand
    {
        public string disableArgument = "-disableFtpUpload";
        
#if ODIN_INSPECTOR
        [BoxGroup("Auth")]
#endif
        public string ftpUrl;
#if ODIN_INSPECTOR
        [BoxGroup("Auth")]
#endif
        public string userName;

#if ODIN_INSPECTOR
        [BoxGroup("Auth")]
#endif
        public string password;

#if ODIN_INSPECTOR
        [ListDrawerSettings(ListElementLabelName = "@Label")]
#endif
        public List<FtpLocation> locations = new List<FtpLocation>()
        {
            new FtpLocation(),
        };

#if ODIN_INSPECTOR
        [BoxGroup("Server Info")]
#endif
        public FtpRemoteExists updateMethod = FtpRemoteExists.Overwrite;

#if ODIN_INSPECTOR
        [BoxGroup("Server Info")]
#endif
        public FtpFolderSyncMode folderSyncMode = FtpFolderSyncMode.Update;

        public override void Execute(IUniBuilderConfiguration configuration)
        {
            if (configuration.Arguments.Contains(disableArgument))
                return;
            
            Upload();
        }

#if ODIN_INSPECTOR
        [Button]
#endif
        public void Upload()
        {
            foreach (var location in locations)
            {
                try
                {
                    Upload(location);
                }
                catch (Exception e)
                {
                    Debug.LogError($"{nameof(AddressablesFtpUploadPostCommand)} Error: {e.Message}");
                }
            }
        }

        public void Upload(FtpLocation location)
        {
            var sourceDirectory = location.sourceDirectory;
            var remoteDirectory = location.remoteDirectory;
            var overrideTargetFolder = location.overrideTargetFolder;
            
            var buildFolder = string.IsNullOrEmpty(sourceDirectory)
                ? location.sourceDirectoryValue
                : sourceDirectory.EvaluateActiveProfileString();
            
            var targetUploadDirectory = remoteDirectory.EvaluateActiveProfileString();

            if (!overrideTargetFolder)
            {
                targetUploadDirectory = Directory.Exists(buildFolder)
                    ? Path.GetFileName(buildFolder)
                    : Path.GetDirectoryName(buildFolder);
            }

            Debug.Log($"FTP Url : {ftpUrl}");
            Debug.Log($"Upload from: {buildFolder}");
            Debug.Log($"Upload to: {targetUploadDirectory}");

            var ftpClient = new FtpClient(ftpUrl);
            ftpClient.Credentials = new NetworkCredential(userName, password, ftpUrl);
            ftpClient.Connect();

            CreateMissingDirectories(targetUploadDirectory, ftpClient);

            var uploadResults = ftpClient.UploadDirectory(buildFolder,
                targetUploadDirectory,
                folderSyncMode,
                updateMethod,
                FtpVerify.None, null, UploadProgress);

            var failed = uploadResults.Where(x => x.IsFailed).ToList();
            var isValidResult = failed.Count <= 0;
            
            if (!isValidResult)
            {
                Debug.LogError($"BuildCommand: {Name} upload to {targetUploadDirectory} failed for:");
                foreach (var ftpResult in failed)
                {
                    Debug.LogError($"{ftpResult.LocalPath} {ftpResult.Size}");
                }
            }

            var uploadResult = isValidResult ? "successfully" : "failed";

            Debug.Log($"BuildCommand: {Name} Upload Complete. result: {uploadResult}");
        }

        private void CreateMissingDirectories(string serverPath, IFtpClient client)
        {
            var pathItems = serverPath.SplitPath();
            var serverLocation = string.Empty;
            foreach (var pathItem in pathItems)
            {
                serverLocation = serverLocation.CombinePath(pathItem);
                var exists = client.DirectoryExists(serverLocation);
                
                if (exists) continue;

                var creationResult = client.CreateDirectory(serverLocation);
                if (creationResult == false)
                {
                    Debug.LogError($"Can't create Remote Folder {serverLocation}");
                }
            }
        }

        private void UploadProgress(FtpProgress progress)
        {
            var progressLog =
                $"Uploading: Source: {progress.LocalPath} Target: {progress.RemotePath}";
            Debug.Log(progressLog);
        }
    }
}