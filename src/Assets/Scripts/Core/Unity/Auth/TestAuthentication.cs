using Core.Auth;
using Core.Basic;
using System;
using System.Threading.Tasks;

namespace Core.Unity.Auth
{
    public class TestAuthentication : IAuthentication
    {
        public string UserId => "test-user-id";
        public string UserName => "test-user";
        public string AuthCode => "test-token";

        public async Task<string> GetAccessToken()
        {
            return "the-token";
        }

        public void SignIn(Action<Result> onDone)
        {
            onDone?.Invoke(Result.Success());
        }

        public async Task<Result> SignIn()
        {
            return Result.Success();
        }
    }
}
