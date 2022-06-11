namespace UniModules.UniGame.BuildCommands.Editor.Addressables
{
    using System;
    using UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
    using UniBuild.Editor.ClientBuild.Interfaces;
    using UnityEngine;

    [Serializable]
    [CreateAssetMenu(menuName = "UniGame/UniBuild/Commands/Addressables/ApplyGroupsTemplateCommand",fileName = nameof(AddressablesApplyGroupsTemplateCommand))]
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
