using Core.Basic;
using System.Threading.Tasks;

namespace Core.Auth
{
    public interface IAuthentication
    {
        Task<string> GetAccessToken();
        Task<Result> SignIn();

        string UserId { get; }
        string UserName { get; }
    }
}
