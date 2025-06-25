using System;
using System.IO;
using System.Linq;
using UniGame.UniBuild.Editor;
using UniModules;
using UnityEditor;
using UnityEngine;


[Serializable]
public class RezipAndroidDebugSymbolsCommand : SerializableBuildCommand
{
    [SerializeField]
    public string debugSymbolsTemplate = "*.symbols.zip";

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.EnumToggleButtons]
#endif
    public AndroidArchitecture androidArchitecture = AndroidArchitecture.All;

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.FilePath]
#endif
    public string zipFileLocation = string.Empty;
    
    public bool removeSourceDebugSymbols = true;
    
    public override void Execute(IUniBuilderConfiguration configuration)
    {
#if UNITY_CLOUD_BUILD
        return;
#endif
        var artifactPath     = configuration.BuildParameters.artifactPath;
        var buildParameters  = configuration.BuildParameters;
        var buildTargetGroup = buildParameters.buildTarget;

        if (buildTargetGroup != BuildTarget.Android)
            return;

        if (string.IsNullOrEmpty(artifactPath))
        {
            Debug.LogWarning($"{nameof(RezipAndroidDebugSymbolsCommand)} ArtifactPath is NULL");
            return;
        }

        Execute(artifactPath, androidArchitecture);
    }
    
    public void Execute(string artifactPath,AndroidArchitecture architecture)
    {
        artifactPath = artifactPath.FixUnityPath();
        var target = SelectDebugSymbolsFile(artifactPath);
        
        if (string.IsNullOrEmpty(target))
        {
            Debug.LogWarning($"{nameof(RezipAndroidDebugSymbolsCommand)} EMPTY Debug Symbols For artofact {artifactPath}");
            return;
        }

        var debugSymbolsPath = target.ToAbsoluteProjectPath();

        try
        {
            AndroidSymbolShrinker.ShrinkSymbols(debugSymbolsPath,architecture);

            if (removeSourceDebugSymbols)
            {
                File.Delete(debugSymbolsPath);
            }
        }
        catch (Exception e)
        {
            return;
        }

    }

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.Button]
#endif
    private void EditorExecute()
    {
        Execute(zipFileLocation,androidArchitecture);
    }

    private string SelectDebugSymbolsFile(string artifactPath)
    {
        var artifactName = Path.GetFileNameWithoutExtension(artifactPath);
        var directory    = Path.GetDirectoryName(artifactPath);
        var files        = Directory.GetFiles(directory, debugSymbolsTemplate, SearchOption.TopDirectoryOnly);
        var target       = files.FirstOrDefault(x => x.IndexOf(artifactName,StringComparison.InvariantCultureIgnoreCase) >= 0);
        return target;
    }
}
