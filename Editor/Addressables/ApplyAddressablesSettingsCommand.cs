using System;
using UniModules.UniGame.UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
using UniModules.UniGame.UniBuild.Editor.ClientBuild.Interfaces;
using UnityEngine;


[Serializable]
public class ApplyAddressablesSettingsCommand : SerializableBuildCommand
{
    public override void Execute(IUniBuilderConfiguration buildParameters)
    {
        Execute();
    }

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.Button]
#endif
    public void Execute()
    {
        
    }
    
}

[Serializable]
public class AddressableBuildSettings
{

    public bool enableEventProfile;
    
    

}