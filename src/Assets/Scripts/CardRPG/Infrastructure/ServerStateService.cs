using System.Net.Http;
using System.Threading.Tasks;

namespace CardRPG.UI.UseCases
{
    public class ServerStateService
    {
        public async Task<bool> IsOnline()
        {
            var client = new HttpClient();

            try
            {
                var result = await client.GetStringAsync("http://localhost:5166/healthz");
                return result == "Healthy";
            }
            catch
            {
                return false;
            }
        }
    }
}
