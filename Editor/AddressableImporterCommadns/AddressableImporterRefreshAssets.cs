using UniGame.UniBuild.Editor;
using UniGame.UniBuild.Editor.Commands;

namespace UniGame.BuildCommands.Editor
{
    using System;
    using System.Collections.Generic;
    using UnityEngine.Scripting.APIUpdating;
    using AddressableImporter = global::AddressableImporter;

     [Serializable]
     [MovedFrom(sourceNamespace:"UniModules.UniGame.BuildCommands.Editor.AddressableImporter")]
     public class AddressableImporterRefreshAssets : SerializableBuildCommand
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
