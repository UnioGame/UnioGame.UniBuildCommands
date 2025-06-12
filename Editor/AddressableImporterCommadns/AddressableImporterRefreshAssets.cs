using UniGame.UniBuild.Editor.ClientBuild.Interfaces;
using UniGame.UniBuild.Editor.Commands.PreBuildCommands;

namespace UniModules.UniGame.BuildCommands.Editor.AddressableImporter
{
    using System;
    using System.Collections.Generic;
    using UnityEngine.Scripting.APIUpdating;
    using AddressableImporter = global::AddressableImporter;

     [Serializable]
     [MovedFrom(true,sourceAssembly:"unigame.build.buildcommands.editor")]
     public class AddressableImporterRefreshAssets : UnitySerializablePreBuildCommand
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FolderPath]
#endif
        public List<string> reimportPaths = new List<string>() {
            "Assets"
        };

        public bool applyCustomRules = true;
        
        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            RefreshAddressables();
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void RefreshAddressables()
        {
            AddressableImporter.FolderImporter.ReimportFolders(reimportPaths,applyCustomRules);
        }
    }
}
