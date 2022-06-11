﻿using UnityEngine;

namespace UniModules.UniGame.BuildCommands.Editor.WebRequests
{
    using UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
    using UniBuild.Editor.ClientBuild.Interfaces;

    [CreateAssetMenu(menuName = "UniGame/UniBuild//CommandsWeb/WebRequestPost",fileName = nameof(WebRequestPostAssetCommand))]
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
