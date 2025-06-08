using UnityEngine;

namespace UniModules.UniGame.BuildCommands.Editor.WebRequests
{
    using global::UniGame.UniBuild.Editor.ClientBuild.Interfaces;
    using global::UniGame.UniBuild.Editor.Commands.PreBuildCommands;

    [CreateAssetMenu(menuName = "UniBuild//CommandsWeb/WebRequestPost",fileName = nameof(WebRequestPostAssetCommand))]
    public class WebRequestPostAssetCommand : UnityPreBuildCommand
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.InlineProperty]
#endif
        public WebRequestPostCommand command = new WebRequestPostCommand();
        
        public override void Execute(IUniBuilderConfiguration configuration)
        {
            command.Execute();
        }
    }
}
