namespace UniGame.BuildCommands.Editor
{
    using System;
    using global::UniGame.AddressableAtlases;
    using global::UniGame.UniBuild.Editor;
    using UniModules.Editor;
    using UnityEngine.Scripting.APIUpdating;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
    [Serializable]
    [MovedFrom(sourceNamespace:"UniModules.UniGame.BuildCommands.Editor.Addressables")]
    public class RebuildAddressablesAtlasesCommand : SerializableBuildCommand
    {
        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            Execute();
        }
        
#if ODIN_INSPECTOR
        [Button]
#endif
        public void Execute()
        {
            var atlasSettings = AssetEditorTools
                .GetAssets<AddressableAtlasesSettingsAsset>();
            foreach (var settings in atlasSettings)
            {
                settings.Reimport();
            }
        }
    }
}
