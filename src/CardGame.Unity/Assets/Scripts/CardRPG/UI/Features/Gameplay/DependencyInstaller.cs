using Core.Auth;
using Core.Net.Http;
using Core.Unity.Auth;
using System;
using Zenject;

namespace CardRPG.UI.Features.Gameplay
{
    public class DependencyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var httpClientManager = new HttpClientManager();
            
            IAuthentication authentication = null;
#if UNITY_EDITOR || UNITY_STANDALONE
            authentication = new FirebaseEmailAuthentication(
                new() { BaseAddress = new Uri("https://identitytoolkit.googleapis.com") }, apiKey: "AIzaSyDz4eKlHC8onnzWU5TzPSYYmhrlPkM6Abc");
#elif UNITY_ANDROID
            authentication = new FirebasePlayGamesAuthentication();
#endif
            Container.Bind<IAuthentication>().FromInstance(authentication).AsSingle();

            var baseAddress = "http://192.168.178.35:5166";
            //var baseAddress = "localhost:5166";
            httpClientManager.CreateClient("public");
            httpClientManager.CreateClient("trinica-public", baseAddress);
            httpClientManager.CreateClient("trinica-authorized", baseAddress, new AuthorizationMessageHandler(authentication));
            Container.Bind<IHttpClientAccessor>().FromInstance(httpClientManager).AsSingle();
        }
    }
}
