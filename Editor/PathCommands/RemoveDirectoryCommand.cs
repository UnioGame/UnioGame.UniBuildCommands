using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniModules.UniGame.Core.Runtime.Extension;
using UniModules.UniGame.UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
using UniModules.UniGame.UniBuild.Editor.ClientBuild.Interfaces;

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
            folderPath.Where(Directory.Exists).ForEach(x => Directory.Delete(x, true));

            assetPath.Where(File.Exists).ForEach(File.Delete);
        }

        public override void Execute(IUniBuilderConfiguration buildParameters) => Execute();
    }
}