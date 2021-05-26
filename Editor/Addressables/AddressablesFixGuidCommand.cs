namespace UniModules.UniBuild.Commands
{
    using System;
    using UniModules.UniGame.UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
    using UniModules.UniGame.UniBuild.Editor.ClientBuild.Interfaces;
    
    [Serializable]
    public class AddressablesFixGuidCommand : UnitySerializablePreBuildCommand
    {
        public override void Execute(IUniBuilderConfiguration buildParameters) => Execute();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        private void Execute() => AddressablesAssetsFix.FixAddressablesGuids();
    
    }

}