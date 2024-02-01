using Core.Basic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Net.Http
{
    public static class HttpClientJsonExtensions
    {
        public static async Task<JsonResponse<TResponseBody, TResponseError>> PostAsJsonAsync<TRequestBody, TResponseBody, TResponseError>(
            this HttpClient client, string resourcePath, TRequestBody body, CancellationToken ct)
        {
            var jsonBody = JsonConvert.SerializeObject(body);
            var response = await client.PostAsync(resourcePath, new StringContent(jsonBody, Encoding.UTF8, "application/json"), ct);

            response.EnsureSuccessStatusCode();

            var jsonResult = await response.Content.ReadAsStringAsync();
            return new JsonResponse<TResponseBody, TResponseError>(
                response,
                BodyString: jsonResult,
                Body: TryCatch.Run(() => JsonConvert.DeserializeObject<TResponseBody>(jsonResult)),
                Error: TryCatch.Run(() => JsonConvert.DeserializeObject<TResponseError>(jsonResult))
            );
        }

        public static async Task<JsonResponse<TResponseError>> PostAsJsonAsync<TRequestBody, TResponseError>(
            this HttpClient client, string resourcePath, TRequestBody body, CancellationToken ct)
        {
            var jsonBody = JsonConvert.SerializeObject(body);
            var response = await client.PostAsync(resourcePath, new StringContent(jsonBody, Encoding.UTF8, "application/json"), ct);

            response.EnsureSuccessStatusCode();

            var jsonResult = await response.Content.ReadAsStringAsync();
            return new JsonResponse<TResponseError>(
                response,
                BodyString: jsonResult,
                Error: TryCatch.Run(() => JsonConvert.DeserializeObject<TResponseError>(jsonResult))
            );
        }

        public static async Task<HttpResponseMessage> PostAsJsonAsync<TRequestBody>(
            this HttpClient client, string resourcePath, TRequestBody body, CancellationToken ct)
        {
            var jsonBody = JsonConvert.SerializeObject(body);
            var response = await client.PostAsync(resourcePath, new StringContent(jsonBody, Encoding.UTF8, "application/json"), ct);

            response.EnsureSuccessStatusCode();

            return response;
        }

        public static async Task<JsonResponse<TResponseBody, TResponseError>> GetAsJsonAsync<TRequestBody, TResponseBody, TResponseError>(
            this HttpClient client, TRequestBody body, string resourcePath, CancellationToken ct)
        {
            var jsonBody = JsonConvert.SerializeObject(body);
            var response = await client.PostAsync(resourcePath, new StringContent(jsonBody, Encoding.UTF8, "application/json"), ct);

            response.EnsureSuccessStatusCode();

            var jsonResult = await response.Content.ReadAsStringAsync();
            return new JsonResponse<TResponseBody, TResponseError>(
                response,
                BodyString: jsonResult,
                Body: TryCatch.Run(() => JsonConvert.DeserializeObject<TResponseBody>(jsonResult)),
                Error: TryCatch.Run(() => JsonConvert.DeserializeObject<TResponseError>(jsonResult))
            );
        }
    }

    public record JsonResponse<TResponseBody, TResponseError>(
        HttpResponseMessage Response,
        string BodyString,
        TResponseBody Body = default,
        TResponseError Error = default);

    public record JsonResponse<TResponseError>(
        HttpResponseMessage Response,
        string BodyString,
        TResponseError Error = default);
}
