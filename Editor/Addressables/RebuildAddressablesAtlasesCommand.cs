namespace UniModules.UniGame.BuildCommands.Editor.Addressables
{
    using System;
    using global::UniGame.AddressableAtlases;
    using global::UniGame.UniBuild.Editor.ClientBuild.Interfaces;
    using global::UniGame.UniBuild.Editor.Commands.PreBuildCommands;
    using UniModules.Editor;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
    [Serializable]
    public class RebuildAddressablesAtlasesCommand : UnitySerializablePreBuildCommand
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
