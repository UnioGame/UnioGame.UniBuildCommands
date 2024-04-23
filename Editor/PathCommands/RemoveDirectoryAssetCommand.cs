using UniModules.UniGame.UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
using UnityEngine;

namespace UniModules.UniBuild.Commands.Editor.PathCommands
{
    using global::UniGame.UniBuild.Editor.ClientBuild.Interfaces;

    [CreateAssetMenu(menuName = "UniBuild/Commands/RemoveDirectory",fileName = nameof(RemoveDirectoryAssetCommand))]
    public class RemoveDirectoryAssetCommand : UnityPreBuildCommand
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
