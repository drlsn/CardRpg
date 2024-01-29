using Core.Basic;
using System.Threading.Tasks;

namespace Core.Unity.Auth
{
    public interface IAuthentication
    {
        Task<string> GetToken();
        Task<Result> SignIn();

        string UserId { get; }
        string UserName { get; }
    }
}
