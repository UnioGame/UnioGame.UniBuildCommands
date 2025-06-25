namespace UniGame.BuildCommands.Editor
{
    using System;
    using global::UniGame.UniBuild.Editor;
    using UnityEngine;
    using UnityEngine.Scripting.APIUpdating;

    [Serializable]
    [MovedFrom(sourceNamespace:"UniModules.UniGame.BuildCommands.Editor.Addressables")]
    public class AddressablesUpdateCatalogVersionCommand : SerializableBuildCommand
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
