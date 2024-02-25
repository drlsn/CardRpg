using CardRPG.UI.Features.Gameplay;
using CardRPG.UI.UseCases;
using Core.Net.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace CardRPG.Infrastructure
{
    internal class OnlineGameplayService : IGameplayService
    {  
        private readonly IHttpClientAccessor _httpClientAccessor;
        private readonly Dictionary<Type, string> _urisPerRequestType = new();

        public OnlineGameplayService(
            IHttpClientAccessor httpClientAccessor, 
            Dictionary<Type, string> urisPerRequestType)
        {
            _httpClientAccessor = httpClientAccessor;
            _urisPerRequestType = urisPerRequestType;
        }

        async Task<TQueryResponse> IGameplayService.Query<TQuery, TQueryResponse>()
        {
            if (!_urisPerRequestType.TryGetValue(typeof(TQuery), out var uri))
                return default;

            var client =_httpClientAccessor.Get(ClientType.TrinicaAuthorized);
            var result = await client.Get<TQueryResponse>(uri);
            if (result.Response.IsSuccessStatusCode)
                return result.Body;

            return default;
        }

        async Task<TQueryResponse> IGameplayService.Query<TQuery, TQueryResponse>(TQuery query)
        {
            if (!_urisPerRequestType.TryGetValue(typeof(TQuery), out var uri))
                return default;

            var parameters = ObjectToUriParams(query);
            uri = $"{uri}?{parameters}";

            var client = _httpClientAccessor.Get(ClientType.TrinicaAuthorized);
            var result = await client.Get<TQueryResponse>(uri);
            if (result.Response.IsSuccessStatusCode)
                return result.Body;

            return default;
        }

        void IGameplayService.Send<TCommand>(TCommand command)
        {
            throw new NotImplementedException();
        }

        void IGameplayService.Subscribe<TEvent>(Action<TEvent> handler)
        {
            throw new NotImplementedException();
        }

        public static string ObjectToUriParams(object obj)
        {
            var keyValuePairs = new List<string>();
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                if (value != null)
                {
                    var encodedValue = HttpUtility.UrlEncode(value.ToString());
                    keyValuePairs.Add($"{property.Name}={encodedValue}");
                }
            }

            return string.Join("&", keyValuePairs);
        }
    }
}
