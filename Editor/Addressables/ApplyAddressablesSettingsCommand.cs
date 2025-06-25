using UniModules.Editor;
using UnityEditor;

namespace UniModules.UniBuild.Commands
{
    using System;
    using global::UniGame.UniBuild.Editor;
    using UnityEditor.AddressableAssets;
    using UnityEngine.Scripting.APIUpdating;

    [Serializable]
    [MovedFrom(sourceNamespace:"UniModules.UniGame.BuildCommands.Editor.Addressables")]
    public class ApplyAddressablesSettingsCommand : SerializableBuildCommand
    {

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#endif
        public AddressableBuildSettings settings = new AddressableBuildSettings();
        
        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            Execute();
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Execute()
        {
            var asset = AddressableAssetSettingsDefaultObject.Settings;
            if (!asset) return;

            asset.ContiguousBundles = settings.contiguousBundles;
            asset.BuildRemoteCatalog = settings.buildRemoteCatalog;
            asset.DisableCatalogUpdateOnStartup = settings.disableCatalogUpdateOnStartUp;
            asset.buildSettings.LogResourceManagerExceptions = settings.logRuntimeErrors;
            asset.UniqueBundleIds = settings.uniqueBundleIds;

#if !UNITY_2023_1_OR_NEWER
            ProjectConfigData.PostProfilerEvents = settings.enableEventProfile;
#endif

            asset.MarkDirty();
            AssetDatabase.Refresh();

        }
    
    }

    [Serializable]
    public class AddressableBuildSettings
    {

        public bool enableEventProfile = false;

        public bool logRuntimeErrors = true;

        public bool disableCatalogUpdateOnStartUp = false;

        public bool buildRemoteCatalog = false;

        public bool uniqueBundleIds = false;

        public bool contiguousBundles = true;

    }
}