namespace UniModules.UniGame.BuildCommands.Editor.AddressableImporter
{
    using System;
    using System.Collections.Generic;
    using global::UniGame.UniBuild.Editor.ClientBuild.Interfaces;
    using UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
    using AddressableImporter = global::AddressableImporter;

    [Serializable]
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
