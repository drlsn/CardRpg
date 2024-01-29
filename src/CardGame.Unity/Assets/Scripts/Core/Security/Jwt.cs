using System;
using System.Text;

namespace Core.Security
{
    public class Jwt
    {
        public static bool ValidateAndDecodeToken(string token)
        {
            var tokenParts = token.Split('.');

            var header = DecodeBase64Url(tokenParts[0]);
            var payload = DecodeBase64Url(tokenParts[1]);

            var expirationTime = GetExpirationTime(payload);

            if (expirationTime.HasValue && expirationTime < DateTime.UtcNow)
                return false;

            return true;
        }

        private static string DecodeBase64Url(string base64Url) =>
            Encoding.UTF8
                .GetString(
                    Convert.FromBase64String(
                        base64Url
                            .PadRight((base64Url.Length + 3) & ~3, '=')
                            .Replace('-', '+').Replace('_', '/')));

        private static DateTime? GetExpirationTime(string payload)
        {
            foreach (var claim in payload.Split(','))
                if (claim.Split(':').Length == 2 && claim.Split(':')[0].Trim('"') == "exp" && long.TryParse(claim.Split(':')[1], out var expUnixTime))
                    return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expUnixTime);

            return null;
        }
    }
}
