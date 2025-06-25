namespace UniGame.BuildCommands.Editor
{
    using System;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine;

    [Serializable]
    public class AddressableTemplateRule
    {
        [SerializeField] public string filter     = String.Empty;
        [SerializeField] public bool   useRegExpr = false;

        [SerializeField] public AddressableAssetGroupTemplate template;
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#endif
        [SerializeField] public AddressablesGroupTemplateOverride groupsOverride = new AddressablesGroupTemplateOverride();

    }
}