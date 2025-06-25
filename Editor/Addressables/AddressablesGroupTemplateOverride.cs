namespace UniGame.BuildCommands.Editor
{
    using System;
    using System.Collections.Generic;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine;

    [Serializable]
    public class AddressablesGroupTemplateOverride
    {
                
        [SerializeField] public bool isOverride = false;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowIf(nameof(isOverride))]
#endif
        [SerializeField] public AddressableAssetGroupTemplate templateOverride;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowIf(nameof(isOverride))]
#endif
        [SerializeField] public List<AddressableAssetGroup> groups;

    }
}