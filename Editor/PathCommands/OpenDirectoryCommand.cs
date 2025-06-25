namespace UniBuild.Commands.Editor
{
    using System;
    using System.Collections.Generic;
    using global::UniGame.UniBuild.Editor;
    using UnityEditor;
    using UnityEngine.Scripting.APIUpdating;

    [Serializable]
    [MovedFrom(sourceNamespace:"UniModules.UniBuild.Commands.Editor.PathCommands")]
    public class OpenDirectoryCommand : SerializableBuildCommand
    {
        public string disableArgument = "-batchmode";
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FolderPath]
#endif
        public List<string> folderPath = new List<string>(){
            "Builds/",    
        };

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Execute()
        {
            foreach (var folder in folderPath)
            {
                EditorUtility.RevealInFinder(folder);
            }
        }

        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            if (buildParameters.Arguments.Contains(disableArgument))
                return;
            
            Execute();
        } 
    }
}