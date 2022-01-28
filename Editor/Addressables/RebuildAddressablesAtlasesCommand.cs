using UniModules.UniGame.CoreModules.UniGame.AddressableTools.Editor.AddressableSpriteAtlasManager;

namespace UniModules.UniGame.BuildCommands.Editor.Addressables
{
    using System;
    using UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
    using UniBuild.Editor.ClientBuild.Interfaces;
    using UnityEngine;

    [Serializable]
    public class RebuildAddressablesAtlasesCommand : UnitySerializablePreBuildCommand
    {
        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            AddressableSpriteAtlasesEditorHandler.Reimport();
        }
    }
}
