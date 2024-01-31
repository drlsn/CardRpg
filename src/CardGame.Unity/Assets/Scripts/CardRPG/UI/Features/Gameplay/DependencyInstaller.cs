using Core.Auth;
using Core.Net.Http;
using Core.Unity.Auth;
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
            authentication = new TestAuthentication();
#else
            authentication = new PlayGamesAuthentication("api/v1/token", httpClientManager);
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
