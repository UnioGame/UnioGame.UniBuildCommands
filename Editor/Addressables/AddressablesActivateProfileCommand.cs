using UnityEngine;
using UniGame.UniBuild.Editor;

namespace UniGame.BuildCommands.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    using UniModules.Editor;
    using UnityEditor;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine.Scripting.APIUpdating;

    [Serializable]
    [MovedFrom(sourceNamespace:"UniModules.UniGame.BuildCommands.Editor.Addressables")]
    public class AddressablesActivateProfileCommand : SerializableBuildCommand
    {
        private AddressableAssetSettings addressableAssetSettings;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ValueDropdown("GetProfiles")]
#endif
        public string targetProfileName = string.Empty;

        public AddressableAssetSettings AddressableAssetSettings => addressableAssetSettings =
            addressableAssetSettings == null ? AssetEditorTools.GetAsset<AddressableAssetSettings>() : addressableAssetSettings;

        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            Execute();
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Execute()
        {
            var settings = AddressableAssetSettings;
            var names    = settings.profileSettings.GetAllProfileNames();
            if (!names.Contains(targetProfileName)) {
                Debug.LogError($"Target profile name doesn't exists for Addressables Settings");
            }

            var targetProfileId = settings.profileSettings.GetProfileId(targetProfileName);
            settings.activeProfileId = targetProfileId;
            settings.MarkDirty();

            Debug.Log($"Activate Addressables Profile {targetProfileName} {settings.activeProfileId}");
            
            AssetDatabase.Refresh();
        }

        private List<string> GetProfiles()
        {
            var settings = AddressableAssetSettings;
            return settings.profileSettings.GetAllProfileNames();
        }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(targetProfileName))
                targetProfileName = GetProfiles()?.FirstOrDefault();
        }
    }
}