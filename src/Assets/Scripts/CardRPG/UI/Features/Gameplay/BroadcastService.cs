using Core.Collections;
using Core.Net.Http;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace CardRPG.UI.Features.Gameplay
{
    internal class BroadcastService 
    {
        private readonly string _accessToken;

        public ConcurrentQueue<string> Messages { get; } = new();

        private readonly IHttpClientAccessor _httpClientAccessor;

        public BroadcastService(IHttpClientAccessor httpClientAccessor, string accessToken)
        {
            _httpClientAccessor = httpClientAccessor;
            _accessToken = accessToken;
        }

        public async Task Do()
        {
            var clientX = _httpClientAccessor.Get(ClientType.TrinicaAuthorizedGameEvents);
            var client = new HttpClient()
            {
                BaseAddress = clientX.BaseAddress,
            };
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");

            while (true)
            {
                try
                {
                    using (var streamReader = new StreamReader(await client.GetStreamAsync("api/v1/games/events")))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            var message = await streamReader.ReadLineAsync();
                            if (!message.IsNullOrEmpty())
                                Messages.Enqueue(message);
                        }
                    }
                    Debug.Log("End");
                    break;
                }
                catch (Exception ex)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            }
        }
    }
}
