namespace UniGame.UniBuild.Editor.Commands.PreBuildCommands.AddressablesCommands
{
    using System;
    using System.Collections.Generic;
    using global::UniGame.UniBuild.Editor.ClientBuild.Interfaces;
    using UnityEditor.AddressableAssets;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine;

#if TRI_INSPECTOR
    using TriInspector;
#endif
    
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
    [Serializable]
    public class AddressablesBuildCommand : UnitySerializablePreBuildCommand
    {
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetBuilders))]
#endif
        public int dataBuilder = 2;

        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            Execute();
        }

#if ODIN_INSPECTOR || TRI_INSPECTOR
        [Button]
#endif
        public void Execute()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            settings.ActivePlayerDataBuilderIndex = dataBuilder;
            AddressableAssetSettings.BuildPlayerContent();
        }

#if ODIN_INSPECTOR
        public IEnumerable<ValueDropdownItem<int>> GetBuilders()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            for (var i = 0; i < settings.DataBuilders.Count; i++)
            {
                var dataBuilder = settings.DataBuilders[i];
                yield return new ValueDropdownItem<int>(dataBuilder.name, i);
            }
        }
#endif
        
    }
}
