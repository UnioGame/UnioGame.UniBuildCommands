using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniGame.Core.Runtime.Extension;
using UnityEditor;
using UnityEngine;

namespace UniBuild.Commands.Editor
{
    using global::UniGame.UniBuild.Editor;
    using UnityEngine.Scripting.APIUpdating;
    using UnityEngine.Serialization;

    [Serializable]
    [MovedFrom(sourceNamespace:"UniModules.UniBuild.Commands.Editor.PathCommands")]
    public class RemoveDirectoryCommand : SerializableBuildCommand
    {
        [FormerlySerializedAs("folderPath")]
        public List<Folder> folders = new List<Folder>();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FilePath]
#endif
        public List<string> assetPath = new List<string>();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Execute()
        {
            foreach (var value in folders)
            {
                if (value.deleteContentOnly)
                {
                    RemoveDirectoryContent(value.folder);
                }
                else
                {
                    RemoveDirectory(value.folder);
                }
            }
            
            AssetDatabase.Refresh();

            assetPath
                .Where(File.Exists)
                .ForEach(x => { TryAction(() => File.Delete(x)); });

            AssetDatabase.Refresh();
        }

        public override void Execute(IUniBuilderConfiguration buildParameters) => Execute();

        public void RemoveDirectory(string folder)
        {
            if (!Directory.Exists(folder)) return;
            
            TryAction(() => FileUtil.DeleteFileOrDirectory(folder));
            
            AssetDatabase.Refresh();
            if (!Directory.Exists(folder)) return;
            
            TryAction(() => Directory.Delete(folder, true));
        }
        
        public void RemoveDirectoryContent(string folder)
        {
            if (!Directory.Exists(folder)) return;
            
            var di = new DirectoryInfo(folder);
            
            foreach (var file in di.GetFiles())
            {
                file.Delete(); 
            }
            
            foreach (var dir in di.GetDirectories())
            {
                dir.Delete(true); 
            }
        }
        
        public bool TryAction(Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return false;
        }
    }

    [Serializable]
    public struct Folder
    {
        public bool deleteContentOnly;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FolderPath]
#endif
        public string folder;
    }
}