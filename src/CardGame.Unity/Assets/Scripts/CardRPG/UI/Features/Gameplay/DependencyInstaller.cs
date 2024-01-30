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
            IAuthentication authentication = null;
#if UNITY_EDITOR || UNITY_STANDALONE
            authentication = new TestAuthentication();
#else
            authentication = new PlayGamesAuthentication();
#endif
            Container.Bind<IAuthentication>().FromInstance(authentication).AsSingle();

            var httpClientManager = new HttpClientManager();
            httpClientManager.CreateClient("public");
            httpClientManager.CreateClient("trinica-authorized", "localhost:5166", new AuthorizationMessageHandler(authentication));
            Container.Bind<IHttpClientAccessor>().FromInstance(httpClientManager).AsSingle();
        }
    }
}
