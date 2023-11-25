namespace UniModules.UniGame.BuildCommands.Editor.Ftp
{
    using System;
    using AddressableExtensions.Editor;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [Serializable]
    public class FtpLocation
    {
#if ODIN_INSPECTOR
        [BoxGroup("Source Info")]
        [InfoBox("You can use addressable variables [BuildPath] e.t.c")]
        [OnValueChanged(nameof(UpdatePreview))]
#endif
        public string sourceDirectory = $"[RemoteBuildPath]";

#if ODIN_INSPECTOR
        [BoxGroup("Source Info")] [ReadOnly]
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
        [BoxGroup("Server Info")] [ShowIf("overrideTargetFolder")] [ReadOnly]
#endif
        public string remoteDirectoryValue = string.Empty;

#if ODIN_INSPECTOR
        [OnInspectorInit]
#endif
        private void UpdatePreview()
        {
            sourceDirectoryValue = sourceDirectory.EvaluateActiveProfileString();
            remoteDirectoryValue = remoteDirectory.EvaluateActiveProfileString();
        }
    }
}