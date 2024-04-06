namespace UniModules.UniGame.UniBuild.Editor.ClientBuild.Commands.PreBuildCommands.AddressablesCommands
{
    using System;
    using global::UniGame.UniBuild.Editor.ClientBuild.Interfaces;
    using Interfaces;
    using UnityEditor.AddressableAssets.Settings;

#if TRI_INSPECTOR
    using TriInspector;
#endif
    
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
    [Serializable]
    public class AddressablesBuildCommand : UnitySerializablePreBuildCommand
    {
        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            Execute();
        }

#if ODIN_INSPECTOR || TRI_INSPECTOR
        [Button]
#endif
        public void Execute()
        {
            AddressableAssetSettings.BuildPlayerContent();
        }
        
    }
}
