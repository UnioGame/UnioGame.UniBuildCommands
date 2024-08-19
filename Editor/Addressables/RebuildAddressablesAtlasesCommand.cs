using UniModules.UniGame.CoreModules.UniGame.AddressableTools.Editor.AddressableSpriteAtlasManager;

namespace UniModules.UniGame.BuildCommands.Editor.Addressables
{
    using System;
    using global::UniGame.UniBuild.Editor.ClientBuild.Interfaces;
    using UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;

    [Serializable]
    public class RebuildAddressablesAtlasesCommand : UnitySerializablePreBuildCommand
    {
        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            AddressableSpriteAtlasesEditorHandler.Reimport();
        }
    }
}
