namespace UniModules.UniBuild.Commands.Editor.PathCommands
{
    using System;
    using System.Collections.Generic;
    using UniGame.UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
    using UniGame.UniBuild.Editor.ClientBuild.Interfaces;
    using UnityEditor;

    [Serializable]
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