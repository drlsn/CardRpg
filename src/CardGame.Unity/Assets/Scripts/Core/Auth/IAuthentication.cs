using Core.Basic;
using System.Threading.Tasks;

namespace Core.Auth
{
    public interface IAuthentication
    {
        Task<string> GetAuthCode();
        Task<Result> SignIn();

        string UserId { get; }
        string UserName { get; }
        string AuthCode { get; }
    }
}
