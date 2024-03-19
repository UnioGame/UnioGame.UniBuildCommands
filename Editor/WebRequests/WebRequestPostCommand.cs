using UniModules.UniCore.Runtime.Extension;

namespace UniModules.UniGame.BuildCommands.Editor.WebRequests
{
    using System;
    using Core.Runtime.Utils;
    using global::UniGame.UniBuild.Editor.ClientBuild.Interfaces;
    using UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
    using UniBuild.Editor.ClientBuild.Interfaces;
    using UnityEngine;
    using UnityEngine.Networking;

    [Serializable]
    public class WebRequestPostCommand : UnitySerializablePostBuildCommand
    {
        public string apiUrl = "";

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.BoxGroup("Parameters")]
#endif
        public WebRequestParameters header = new WebRequestParameters() {
            {"Content-Type","application/json"},
            {"Accept","application/json"},
        };
        
        [Space(4)]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.BoxGroup("Parameters")]
#endif
        public WebRequestParameters parameters = new WebRequestParameters();
        
        public override void Execute(IUniBuilderConfiguration configuration) => Execute();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Execute()
        {
            var targetUrl = apiUrl.CombineUrlParameters(parameters);

#if UNITY_2022_2_OR_NEWER
            var webRequest = UnityWebRequest.PostWwwForm(targetUrl,string.Empty);
#else
            var webRequest = UnityWebRequest.Post(targetUrl,string.Empty);
#endif
            foreach (var headerParameter in header) {
                webRequest.SetRequestHeader(headerParameter.Key,headerParameter.Value);
            }
            
            Debug.Log($"Send Post to : {webRequest.uri}");
            
            var requestAsyncOperation = webRequest.SendWebRequest();
            requestAsyncOperation.completed += x => {

                if (webRequest.result ==UnityWebRequest.Result.ConnectionError || 
                    webRequest.result ==UnityWebRequest.Result.ProtocolError || 
                    webRequest.result ==UnityWebRequest.Result.DataProcessingError) {
                    Debug.Log(webRequest.error);
                }
                else {
                    Debug.Log($"Request to {apiUrl} complete. Code: {webRequest.responseCode}");
                }

                webRequest.Cancel();
            };
        }
    }
}
