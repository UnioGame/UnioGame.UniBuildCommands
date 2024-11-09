namespace UniModules.UniGame.BuildCommands.Editor.Addressables
{
    using System;
    using global::UniGame.AddressableAtlases;
    using global::UniGame.UniBuild.Editor.ClientBuild.Interfaces;
    using Sirenix.OdinInspector;
    using UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
    using UniModules.Editor;

    [Serializable]
    public class RebuildAddressablesAtlasesCommand : UnitySerializablePreBuildCommand
    {
        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            Execute();
        }
        
        [Button]
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
