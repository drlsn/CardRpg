using Core.Basic;
using Core.Net.Http;
using Corelibs.Basic;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Corelibs.BlazorShared
{
    public static class HttpRequestExtensions
    {
        //public static async Task<TResponse> GetResource<TResponse>(
        //    this IHttpClientAccessor clientAccessor, string clientName, string resourcePath, CancellationToken ct = default)
        //{
        //    try
        //    {
        //        return await clientAccessor.GetAsync<TResponse>(AuthUserTypes.Authorized, resourcePath, ct);
        //    }
        //    catch (NoAccessTokenAvailableException accessTokenNotAvailableException)
        //    {
        //        try
        //        {
        //            return await clientAccessor.GetAsync<TResponse>(AuthUserTypes.Anonymous, resourcePath, ct);
        //        }
        //        catch (HttpRequestException ex)
        //        {
        //            return default;

        //            //if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //            //{
        //            //    signInRedirector.Redirect(accessTokenNotAvailableException);
        //            //    return default;
        //            //}
        //            //throw ex;
        //        }
        //        catch (Exception ex)
        //        {
        //            return default;

        //            //Console.WriteLine(ex);
        //            //return default;
        //        }
        //    }
        //}

        record EmptyBody();

        public static Task<HttpResponseMessage> PostResource(
            this IHttpClientAccessor clientAccessor,
            string authorizedClientName,
            string anonymousClientName,
            string resourcePath, 
            CancellationToken ct = default)
        {
            return SendResource(
                authorizedClientName,
                anonymousClientName,
                clientName => clientAccessor.PostAsync(clientName, resourcePath, new EmptyBody(), ct));
        }

        public static Task<HttpResponseMessage> PostResource<TBody>(
            this IHttpClientAccessor clientAccessor,
            string authorizedClientName,
            string anonymousClientName,
            string resourcePath,
            TBody body,
            CancellationToken ct = default)
        {
            return SendResource(
                authorizedClientName,
                anonymousClientName,
                clientName => clientAccessor.PostAsync(clientName, resourcePath, body, ct));
        }

        public static Task<JsonResponse<TResponse>> PostResource<TBody, TResponse>(
            this IHttpClientAccessor clientAccessor,
            string authorizedClientName,
            string anonymousClientName,
            string resourcePath,
            TBody body,
            CancellationToken ct = default)
        {
            return SendResource(
                authorizedClientName,
                anonymousClientName,
                clientName => clientAccessor.PostAsync<TBody, TResponse>(clientName, resourcePath, body, ct));
        }

        public static Task<JsonResponseOrError<TResponse, TResponseError>> PostResource<TBody, TResponse, TResponseError>(
            this IHttpClientAccessor clientAccessor,
            string authorizedClientName,
            string anonymousClientName,
            string resourcePath,
            TBody body,
            CancellationToken ct = default)
        {
            return SendResource(
                authorizedClientName,
                anonymousClientName,
                clientName => clientAccessor.PostAsync<TBody, TResponse, TResponseError>(clientName, resourcePath, body, ct));
        }

        public static Task<JsonResponseOrError<TResponse, TResponseError>> PostResource<TBody, TResponse, TResponseError>(
            this IHttpClientAccessor clientAccessor,
            string clientName,
            string resourcePath,
            TBody body,
            CancellationToken ct = default)
        {
            return TryCatch.Run(() => clientAccessor.PostAsync<TBody, TResponse, TResponseError>(clientName, resourcePath, body, ct));
        }

        //public static Task<HttpResponseMessage> PutResource<TBody>(
        //   this IHttpClientAccessor clientAccessor, string resourcePath, TBody body, CancellationToken ct)
        //{
        //    return SendResource(clientName => clientAccessor.PutAsync(clientName, resourcePath, body, ct));
        //}

        //public static Task<HttpResponseMessage> PatchResource<TBody>(
        //   this IHttpClientAccessor clientAccessor, string resourcePath, TBody body, CancellationToken ct)
        //{
        //    return SendResource(clientName => clientAccessor.PatchAsync(clientName, resourcePath, body, ct));
        //}

        //public static Task<HttpResponseMessage> DeleteResource(
        //  this IHttpClientAccessor clientAccessor, string resourcePath, CancellationToken ct)
        //{
        //    return SendResource(clientName => clientAccessor.DeleteAsync(clientName, resourcePath, ct));
        //}

        //public static Task<HttpResponseMessage> DeleteResource<TBody>(
        //  this IHttpClientAccessor clientAccessor, string resourcePath, TBody body, CancellationToken ct)
        //{
        //    return SendResource(clientName => clientAccessor.DeleteAsync(clientName, resourcePath, body, ct));
        //}

        private static async Task<T> SendResource<T>(
            string authorizedClientName,
            string anonymousClientName,
            Func<string, Task<T>> sendResourceFunc)
        {
            try
            {
                return await sendResourceFunc(authorizedClientName);
            }
            catch (NoAccessTokenAvailableException noAccessTokenAvailableException)
            {
                if (authorizedClientName == anonymousClientName)
                    return default;

                try
                {
                    return await sendResourceFunc(anonymousClientName);
                }
                catch (HttpRequestException ex)
                {
                    //if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    //{
                    //    signInRedirector.Redirect(noAccessTokenAvailableException);
                    //    return default;
                    //}

                    throw ex;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return default;
                }
            }
        }

        //private static Task<TResponse> GetAsync<TResponse>(this IHttpClientAccessor clientAccessor, string clientName, string resourcePath, CancellationToken ct)
        //{
        //    var client = clientAccessor.Get(clientName);
        //    return HttpClientJsonExtensions.GetFromJsonAsync<TResponse>(client, resourcePath, ct);
        //}

        private static Task<HttpResponseMessage> PostAsync<TBody>(
            this IHttpClientAccessor clientAccessor, string clientName, string resourcePath, TBody body, CancellationToken ct)
        {
            return clientAccessor.CreateClientAndSendRequest(
                clientName, client => HttpClientJsonExtensions.PostAsJson(client, resourcePath, body, ct));
        }

        private static Task<JsonResponse<TResponse>> PostAsync<TBody, TResponse>(
           this IHttpClientAccessor clientAccessor, string clientName, string resourcePath, TBody body, CancellationToken ct)
        {
            return clientAccessor.CreateClientAndSendRequest(
                clientName, client => HttpClientJsonExtensions.PostAsJson<TBody, TResponse>(client, resourcePath, body, ct));
        }

        private static Task<JsonResponseOrError<TResponse, TResponseError>> PostAsync<TBody, TResponse, TResponseError>(
            this IHttpClientAccessor clientAccessor, string clientName, string resourcePath, TBody body, CancellationToken ct)
        {
            return clientAccessor.CreateClientAndSendRequest(
                clientName, client => HttpClientJsonExtensions.PostAsJsonExpectError<TBody, TResponse, TResponseError>(client, resourcePath, body, ct));
        }

        public static Task<JsonResponse<TResponse>> GetAsync<TResponse>(
           this IHttpClientAccessor clientAccessor, string clientName, string resourcePath, CancellationToken ct = default)
        {
            return clientAccessor.CreateClientAndSendRequest(
                clientName, client => HttpClientJsonExtensions.Get<TResponse>(client, resourcePath, ct));
        }

        //private static Task<HttpResponseMessage> PutAsync<TBody>(this IHttpClientAccessor clientAccessor, string clientName, string resourcePath, TBody body, CancellationToken ct) =>
        //    clientAccessor.CreateClientAndSendRequest(clientName, client => HttpClientJsonExtensions.PutAsJsonAsync(client, resourcePath, body, ct));

        //private static Task<HttpResponseMessage> PatchAsync<TBody>(this IHttpClientAccessor clientAccessor, string clientName, string resourcePath, TBody body, CancellationToken ct) =>
        //    clientAccessor.CreateClientAndSendRequest(clientName, client => client.PatchAsync(resourcePath, new ObjectContent(typeof(TBody), body, new JsonMediaTypeFormatter(), "application/json"), ct));

        //private static Task<HttpResponseMessage> DeleteAsync(this IHttpClientAccessor clientAccessor, string clientName, string resourcePath, CancellationToken ct) =>
        //    clientAccessor.CreateClientAndSendRequest(clientName, client => client.DeleteAsync(resourcePath, ct));

        //private static Task<HttpResponseMessage> DeleteAsync<TBody>(
        //    this IHttpClientAccessor clientAccessor, string clientName, string resourcePath, TBody body, CancellationToken ct) =>
        //    clientAccessor.CreateClientAndSendRequest(clientName, client =>
        //    {
        //        var request = new HttpRequestMessage
        //        {
        //            Content = body,
        //            Method = HttpMethod.Delete,
        //            RequestUri = new Uri(resourcePath, UriKind.Relative)
        //        };

        //        return client.SendAsync(request, ct);
        //    });

        private static Task<T> CreateClientAndSendRequest<T>(
            this IHttpClientAccessor clientAccessor, string clientName, Func<HttpClient, Task<T>> request)
        {
            var client = clientAccessor.Get(clientName);
            return request(client);
        }
    }
}
