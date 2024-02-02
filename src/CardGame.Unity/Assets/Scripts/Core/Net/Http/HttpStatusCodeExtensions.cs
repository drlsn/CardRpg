using System.Net;

namespace Core.Net.Http
{
    public static class HttpStatusCodeExtensions
    {
        public static bool IsSuccess(this HttpStatusCode statusCode)
        {
            int code = (int)statusCode;
            return code >= 200 && code < 300;
        }
    }
}
