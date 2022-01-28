using System;
using UniModules.UniGame.CoreModules.UniGame.AddressableTools.Editor.Commands;
using UniModules.UniGame.UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
using UniModules.UniGame.UniBuild.Editor.ClientBuild.Interfaces;

namespace UniModules.UniBuild.Commands.Editor.Addressables
{
    [Serializable]
    public class AddressablesFixGuidCommand : UnitySerializablePreBuildCommand
    {
        public override void Execute(IUniBuilderConfiguration buildParameters) => Execute();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        private void Execute() => AddressablesAssetsFix.FixAddressablesErrors();
    }
}
