namespace UniGame.BuildCommands.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using UniModules.Editor;
    using UnityEditor;
    using UnityEditor.AddressableAssets;
    using UnityEngine;

    [Serializable]
    public class ApplyAddressablesTemplatesCommand
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.ListDrawerSettings(Expanded = true,DraggableItems = true,ShowPaging = true)]
#endif
        public List<AddressableTemplateRule> groupRules = new List<AddressableTemplateRule>();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Execute()
        {
            foreach (var rule in groupRules)
            {
                ApplyTemplate(rule);
            }
            
            AssetDatabase.SaveAssets();
        }

        private void ApplyTemplate(AddressableTemplateRule rule)
        {
            var filter       = rule.filter;
            var overrideData = rule.groupsOverride;
            var useOverride  = overrideData.isOverride;
            var regExprValue = new Regex(filter,RegexOptions.Compiled|RegexOptions.IgnoreCase);


            var settings = AddressableAssetSettingsDefaultObject.Settings;
            
            if (settings == null) {
                Debug.LogError("Addressable assets settings not found");
                return;
            }

            var groups = settings.
                groups.
                Where(g => rule.useRegExpr ? 
                    regExprValue.IsMatch(g.Name) : 
                    g.Name.StartsWith(filter)).
                ToList();
            
            foreach (var group in groups)
            {
                var template = useOverride &&  overrideData.groups.Contains(group) ? 
                    overrideData.templateOverride : 
                    rule.template;
                template.ApplyToAddressableAssetGroup(group);
                group.MarkDirty();
            }
        }
    }
}