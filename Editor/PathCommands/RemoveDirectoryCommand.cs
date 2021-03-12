using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniModules.UniGame.Core.Runtime.Extension;
using UniModules.UniGame.UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
using UniModules.UniGame.UniBuild.Editor.ClientBuild.Interfaces;
using UnityEditor;
using UnityEngine;

namespace UniModules.UniBuild.Commands.Editor.PathCommands
{
    [Serializable]
    public class RemoveDirectoryCommand : UnitySerializablePreBuildCommand
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FolderPath]
#endif
        public List<string> folderPath = new List<string>();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FilePath]
#endif
        public List<string> assetPath = new List<string>();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Execute()
        {
            folderPath
                .Where(Directory.Exists)
                .ForEach(x =>
                {
                    TryAction(() => FileUtil.DeleteFileOrDirectory(x));
                });
            
            AssetDatabase.Refresh();
            
            folderPath
                .Where(Directory.Exists)
                .ForEach(x =>
                {
                    TryAction(() => Directory.Delete(x, true));
                    
                });
            
            AssetDatabase.Refresh();
            
            assetPath
                .Where(File.Exists)
                .ForEach(x =>
                {
                    TryAction(() => File.Delete(x));
                });

            AssetDatabase.Refresh();
        }

        public override void Execute(IUniBuilderConfiguration buildParameters) => Execute();

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
}