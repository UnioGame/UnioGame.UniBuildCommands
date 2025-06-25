using UnityEngine;

namespace UniBuild.Commands.Editor
{
    using global::UniGame.UniBuild.Editor;
    using global::UniGame.UniBuild.Editor.Commands;
    using UnityEngine.Scripting.APIUpdating;

    [CreateAssetMenu(menuName = "UniBuild/Commands/RemoveDirectory",fileName = nameof(RemoveDirectoryAssetCommand))]
    [MovedFrom(sourceNamespace:"UniModules.UniBuild.Commands.Editor.PathCommands")]
    public class RemoveDirectoryAssetCommand : UnityBuildCommand
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.InlineProperty]
#endif
        public RemoveDirectoryCommand command = new RemoveDirectoryCommand();
        
        public override void Execute(IUniBuilderConfiguration configuration)
        {
            command.Execute();
        }
    }
}
