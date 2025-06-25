namespace UniGame.BuildCommands.Editor
{
    using System;
    using global::UniGame.UniBuild.Editor;
    using UnityEngine;
    using UnityEngine.Scripting.APIUpdating;

    [Serializable]
    [MovedFrom(sourceNamespace:"UniModules.UniGame.BuildCommands.Editor.Addressables")]
    public class ApplyAddressablesGroupsTemplateCommand : SerializableBuildCommand
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#endif
        [SerializeField]
        private ApplyAddressablesTemplatesCommand command = new ApplyAddressablesTemplatesCommand();

        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            command.Execute();
        }

        
    }
}
