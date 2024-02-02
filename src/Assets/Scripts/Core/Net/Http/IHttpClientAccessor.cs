using System.Net.Http;

namespace Core.Net.Http
{
    public interface IHttpClientAccessor
    {
        HttpClient Get(string name);
    }
}
