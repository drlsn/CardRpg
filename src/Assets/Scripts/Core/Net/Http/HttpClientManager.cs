using Core.Collections;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Core.Net.Http
{
    public class HttpClientManager : IHttpClientAccessor
    {
        private readonly Dictionary<string, HttpClient> _clients = new();

        public HttpClient CreateClient(
            string name, string baseAddress = "", HttpMessageHandler messageHandler = null)
        {
            HttpClient client = messageHandler is null ? new() : new(messageHandler);
            if (!baseAddress.IsNullOrEmpty())
                client.BaseAddress = new Uri(baseAddress);

            _clients.Add(name, client);

            return client;
        }

        public HttpClient Get(string name) =>
            _clients[name];
    }
}
