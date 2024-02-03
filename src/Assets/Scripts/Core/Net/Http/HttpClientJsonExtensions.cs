using Core.Basic;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Net.Http
{
    public static class HttpClientJsonExtensions
    {
        public static async Task<JsonResponseOrError<TResponseBody, TResponseError>> PostAsJsonExpectError<TRequestBody, TResponseBody, TResponseError>(
            this HttpClient client, string resourcePath, TRequestBody body, CancellationToken ct = default)
        {
            var jsonBody = JsonConvert.SerializeObject(body);
            var response = await client.PostAsync(resourcePath, new StringContent(jsonBody, Encoding.UTF8, "application/json"), ct);

            var jsonResult = await response.Content.ReadAsStringAsync();
            return new JsonResponseOrError<TResponseBody, TResponseError>(
                response,
                BodyString: jsonResult,
                Body: TryCatch.Run(() => JsonConvert.DeserializeObject<TResponseBody>(jsonResult)),
                Error: TryCatch.Run(() => JsonConvert.DeserializeObject<TResponseError>(jsonResult))
            );
        }

        public static async Task<JsonResponseOrError<TResponseError>> PostAsJsonExpectError<TRequestBody, TResponseError>(
            this HttpClient client, string resourcePath, TRequestBody body, CancellationToken ct = default)
        {
            var jsonBody = JsonConvert.SerializeObject(body);
            var response = await client.PostAsync(resourcePath, new StringContent(jsonBody, Encoding.UTF8, "application/json"), ct);

            var jsonResult = await response.Content.ReadAsStringAsync();
            return new JsonResponseOrError<TResponseError>(
                response,
                BodyString: jsonResult,
                Error: TryCatch.Run(() => JsonConvert.DeserializeObject<TResponseError>(jsonResult))
            );
        }

        public static async Task<JsonResponse<TResponse>> PostAsJson<TRequestBody, TResponse>(
            this HttpClient client, string resourcePath, TRequestBody body, CancellationToken ct = default)
        {
            var jsonBody = JsonConvert.SerializeObject(body);
            var response = await client.PostAsync(resourcePath, new StringContent(jsonBody, Encoding.UTF8, "application/json"), ct);

            var jsonResult = await response.Content.ReadAsStringAsync();
            return new JsonResponse<TResponse>(
                response,
                BodyString: jsonResult,
                Body: TryCatch.Run(() => JsonConvert.DeserializeObject<TResponse>(jsonResult))
            );
        }

        public static async Task<HttpResponseMessage> PostAsJson<TRequestBody>(
            this HttpClient client, string resourcePath, TRequestBody body, CancellationToken ct = default)
        {
            var jsonBody = JsonConvert.SerializeObject(body);
            var response = await client.PostAsync(resourcePath, new StringContent(jsonBody, Encoding.UTF8, "application/json"), ct);

            return response;
        }

        public static async Task<HttpResponseMessage> Post(
            this HttpClient client, string resourcePath, CancellationToken ct = default)
        {
            return await client.PostAsync(resourcePath, new StringContent("", Encoding.UTF8, "application/json"), ct);
        }

        public static async Task<JsonResponseOrError<TResponseBody, TResponseError>> GetAsJson<TRequestBody, TResponseBody, TResponseError>(
            this HttpClient client, TRequestBody body, string resourcePath, CancellationToken ct = default)
        {
            var jsonBody = JsonConvert.SerializeObject(body);
            var response = await client.PostAsync(resourcePath, new StringContent(jsonBody, Encoding.UTF8, "application/json"), ct);

            var jsonResult = await response.Content.ReadAsStringAsync();
            return new JsonResponseOrError<TResponseBody, TResponseError>(
                response,
                BodyString: jsonResult,
                Body: TryCatch.Run(() => JsonConvert.DeserializeObject<TResponseBody>(jsonResult)),
                Error: TryCatch.Run(() => JsonConvert.DeserializeObject<TResponseError>(jsonResult))
            );
        }
    }

    public record JsonResponseOrError<TResponse, TResponseError>(
        HttpResponseMessage Response,
        string BodyString,
        TResponse Body = default,
        TResponseError Error = default);

    public record JsonResponseOrError<TResponseError>(
        HttpResponseMessage Response,
        string BodyString,
        TResponseError Error = default);

    public record JsonResponse<TResponse>(
        HttpResponseMessage Response,
        string BodyString,
        TResponse Body = default);
}
