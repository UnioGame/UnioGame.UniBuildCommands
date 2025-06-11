namespace UniModules.UniGame.BuildCommands.Editor.Ftp
{
    using System;
    using global::UniGame.AddressableTools.Editor;
    using UnityEngine;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    [Serializable]
    public class FtpLocation
    { 
#if ODIN_INSPECTOR    
        [BoxGroup("Source Info")]
#endif
        public bool overrideSourceDirectory = false;
            
#if ODIN_INSPECTOR
        [BoxGroup("Source Info")]
        [InfoBox("You can use addressable variables [BuildPath] e.t.c")]
        [ShowIf(nameof(overrideSourceDirectory))]
        [OnValueChanged(nameof(UpdatePreview))]
#endif
        public string sourceDirectory = $"Remote.BuildPath";

#if ODIN_INSPECTOR
        [BoxGroup("Source Info")]
        [ReadOnly]
        [LabelText("source")]
#endif
        public string sourceDirectoryValue = string.Empty;

#if ODIN_INSPECTOR
        [BoxGroup("Server Info")]
#endif
        public bool overrideTargetFolder = false;

#if ODIN_INSPECTOR
        [BoxGroup("Server Info")]
        [ShowIf("overrideTargetFolder")]
        [InfoBox("You can use addressable variables [BuildPath] e.t.c")]
#endif
        [Space(6)]
        public string remoteDirectory = string.Empty;

#if ODIN_INSPECTOR
        [BoxGroup("Server Info")]
        [ReadOnly]
        [LabelText("remote")]
#endif
        public string remoteDirectoryValue = string.Empty;

        public string Label => string.IsNullOrEmpty(sourceDirectoryValue)
            ? sourceDirectory
            : sourceDirectoryValue;

#if ODIN_INSPECTOR
        [OnInspectorInit]
#endif
        private void UpdatePreview()
        {
            sourceDirectoryValue = overrideSourceDirectory
                    ? sourceDirectory.EvaluateActiveProfileString()
                    : AddressableEditorTools.GetRemoteBuildPath();
            
            remoteDirectoryValue = overrideTargetFolder
                    ? remoteDirectory.EvaluateActiveProfileString()
                    : AddressableEditorTools.GetRemoteLoadPath();
        }
    }
}