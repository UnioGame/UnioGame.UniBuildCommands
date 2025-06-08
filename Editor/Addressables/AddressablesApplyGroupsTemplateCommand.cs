namespace UniModules.UniGame.BuildCommands.Editor.Addressables
{
    using System;
    using global::UniGame.UniBuild.Editor.ClientBuild.Interfaces;
    using global::UniGame.UniBuild.Editor.Commands.PreBuildCommands;
    using UnityEngine;

    [Serializable]
    [CreateAssetMenu(menuName = "UniBuild/Commands/Addressables/ApplyGroupsTemplateCommand",fileName = nameof(AddressablesApplyGroupsTemplateCommand))]
    public class AddressablesApplyGroupsTemplateCommand : UnityPreBuildCommand
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#endif
        [SerializeField]
        private ApplyAddressablesTemplatesCommand command = new ApplyAddressablesTemplatesCommand();

        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            Execute();
        }
        
        public void Execute()
        {
            command.Execute();
        }
        
    }
}
