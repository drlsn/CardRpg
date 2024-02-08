using Core.Auth;
using Core.Net.Http;
using Core.Unity.Auth;
using System;
using System.Net;
using UnityEngine;
using Zenject;

namespace CardRPG.UI.Features.Gameplay
{
    public class DependencyInstaller : MonoInstaller
    {
        [SerializeField] private string _gameServerIP = "192.168.178.35";
        [SerializeField] private int _gameServerPort = 5166;

        public override void InstallBindings()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var baseAddress = $"http://{_gameServerIP}:{_gameServerPort}";

            var httpClientManager = new HttpClientManager();
            httpClientManager.CreateClient(ClientType.Public);
            httpClientManager.CreateClient(ClientType.TrinicaPublic, baseAddress);
            Container.Bind<IHttpClientAccessor>().FromInstance(httpClientManager).AsSingle();

            IAuthentication authentication = null;
#if UNITY_EDITOR || UNITY_STANDALONE
            authentication = new CustomServerAuthentication(
                new FirebaseEmailAuthentication(
                    new() { BaseAddress = new Uri("https://identitytoolkit.googleapis.com") }, apiKey: "AIzaSyDz4eKlHC8onnzWU5TzPSYYmhrlPkM6Abc"),
                httpClientManager,
                ClientType.TrinicaAuthorized);
#elif UNITY_ANDROID
            authentication = new CustomServerAuthentication(
                new FirebasePlayGamesAuthentication(),
                httpClientManager,
                ClientType.TrinicaAuthorized);
#endif
            Container.Bind<IAuthentication>().FromInstance(authentication).AsSingle();

            httpClientManager.CreateClient(ClientType.TrinicaAuthorized, baseAddress, new AuthorizationMessageHandler(authentication));
            httpClientManager.CreateClient(ClientType.TrinicaAuthorizedGameEvents, baseAddress, new AuthorizationMessageHandler(authentication));
        }
    }
}
