using UniModules.UniGame.AddressableExtensions.Editor;

namespace UniModules.UniGame.BuildCommands.Editor.Addressables
{
    using System;
    using UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
    using UniBuild.Editor.ClientBuild.Interfaces;
    using UnityEditor.Build.Pipeline.Utilities;
    using UnityEngine;
    using BuildLogger = UniBuild.Editor.ClientBuild.BuildLogger;

    public enum CleanType
    {
        CleanAll,
        CleanContentBuilders,
        CleanBuildPipelineCache,
    }
    
    [Serializable]
    public class AddressablesCleanUpCommand : UnitySerializablePreBuildCommand
    {
        public string CleanUpArgument = "-addressableCleanUp";
        
        [Tooltip("Clean Addressables Library cache")]
        public bool CleanUpLibraryCache = true;
        
        [Tooltip("Clean Addressables Library cache")]
        public bool CleanUpStreamingCache = true;

        public CleanType CleanType = CleanType.CleanAll;
        
        public bool promtWarning = false;
        
        private bool _forceCleanUp = false;
        
        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            _forceCleanUp = buildParameters.Arguments.Contains(CleanUpArgument);
            
            BuildLogger.LogWithTimeTrack($"CleanUpArgument: {CleanUpArgument} ==  {_forceCleanUp}");
            
            Execute();
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Execute()
        {
            if (CleanUpLibraryCache || _forceCleanUp)
                AddressablesCleaner.RemoveLibraryCache();

            if (CleanUpStreamingCache || _forceCleanUp)
                AddressablesCleaner.RemoveStreamingCache();

            var cleanType = _forceCleanUp ? CleanType : CleanType.CleanAll;
            
            switch (cleanType) {
                case CleanType.CleanAll:
                    CleanAll();
                    break;
                case CleanType.CleanContentBuilders:
                    AddressablesCleaner.CleanPlayerContent(null);
                    break;
                case CleanType.CleanBuildPipelineCache:
                    OnCleanSBP();
                    break;
            }
        }
        
        public void CleanAll()
        {
            AddressablesCleaner.CleanAll();
            OnCleanSBP();
        }

        public void OnCleanSBP()
        {
            BuildCache.PurgeCache(promtWarning);
        }
        
    }
}
