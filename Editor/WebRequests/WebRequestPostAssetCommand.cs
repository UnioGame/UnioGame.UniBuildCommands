using UnityEngine;

namespace UniGame.BuildCommands.Editor
{
    using global::UniGame.UniBuild.Editor;
    using global::UniGame.UniBuild.Editor.Commands;
    using UnityEngine.Scripting.APIUpdating;

    [CreateAssetMenu(menuName = "UniBuild//CommandsWeb/WebRequestPost",fileName = nameof(WebRequestPostAssetCommand))]
    [MovedFrom(sourceNamespace:"UniModules.UniGame.BuildCommands.Editor.WebRequests")]
    public class WebRequestPostAssetCommand : UnityBuildCommand
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
