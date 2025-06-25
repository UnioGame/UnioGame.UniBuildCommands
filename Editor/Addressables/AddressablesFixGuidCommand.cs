using System;
using UniGame.AddressableTools.Editor;
using global::UniGame.UniBuild.Editor;

namespace UniBuild.Commands.Editor
{
    using UnityEngine.Scripting.APIUpdating;

    [Serializable]
    [MovedFrom(sourceNamespace:"UniModules.UniBuild.Commands.Editor.Addressables")]
    public class AddressablesFixGuidCommand : SerializableBuildCommand
    {
        public override void Execute(IUniBuilderConfiguration buildParameters) => Execute();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        private void Execute() => AddressablesAssetsFix.FixAddressablesErrors();
    }
}
