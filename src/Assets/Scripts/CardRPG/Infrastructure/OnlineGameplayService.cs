using CardRPG.UI.Features.Gameplay;
using CardRPG.UI.UseCases;
using Core.Collections;
using Core.Net.Http;
using ModestTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        Task<TQueryResponse> IGameplayService.Query<TQuery, TQueryResponse>() =>
            (this as IGameplayService).Query<TQuery, TQueryResponse>(new TQuery());

        async Task<TQueryResponse> IGameplayService.Query<TQuery, TQueryResponse>(TQuery query)
        {
            if (!_urisPerRequestType.TryGetValue(typeof(TQuery), out var uri))
                return default;

            var routeParameters = GetRouteParameters(query, out var @params);
            var queryParams = ObjectToUriParams(query);

            uri = uri.Replace("{", "").Replace("}", "");
            foreach (var param in @params)
                uri = uri.Replace(param.Key, param.Value);

            uri = $"{uri}{routeParameters}{queryParams}";

            var client = _httpClientAccessor.Get(ClientType.TrinicaAuthorized);
            var result = await client.Get<TQueryResponse>(uri);
            if (result.Response.IsSuccessStatusCode)
                return result.Body;

            return default;
        }

        async Task<bool> IGameplayService.Send<TCommand>(TCommand command)
        {
            if (!_urisPerRequestType.TryGetValue(typeof(TCommand), out var uri))
                return false;

            var client = _httpClientAccessor.Get(ClientType.TrinicaAuthorized);
            var response = await client.PostAsJson(uri, command);
            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }

        Task<bool> IGameplayService.Send<TCommand>() =>
            (this as IGameplayService).Send(new TCommand());

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
                if (value != null && !Attribute.IsDefined(property, typeof(FromRouteAttribute)))
                {
                    var encodedValue = HttpUtility.UrlEncode(value.ToString());
                    keyValuePairs.Add($"{property.Name}={encodedValue}");
                }
            }

            return string.Join("&", keyValuePairs);
        }

        public static string GetRouteParameters<TQuery>(TQuery query, out Dictionary<string, string> parameters)
        {
            var type = typeof(TQuery);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var propertiesWithRoute = properties.Where(prop => prop.GetCustomAttributes().Any(a => a.GetType() == typeof(FromRouteAttribute)));
            var propertyValues = propertiesWithRoute.Select(prop => new { Name = prop.Name.ToCamelCase(), Value = prop.GetValue(query) }).ToArray();

            parameters = new();
            foreach (var p in propertyValues)
                parameters.Add(p.Name, p.Value as string);

            return 
                (properties.Count() > 0 ? "/" : "") + 
                string.Join("/", properties.Select(p => p.Name)) +
                (properties.Count() > 0 ? "?" : "");
        }
    }
}
