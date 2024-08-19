namespace UniModules.UniGame.BuildCommands.Editor.Addressables
{
    using System;
    using global::UniGame.UniBuild.Editor.ClientBuild.Interfaces;
    using UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
    using UnityEngine;

    [Serializable]
    public class AddressablesUpdateCatalogVersionCommand : UnitySerializablePreBuildCommand
    {
        public bool   useAppVersion = true;
        public bool   useBuildNumber = false;
        public string manualVersion = Application.version;
        
        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            var version = Application.version;
        }
    }
}
