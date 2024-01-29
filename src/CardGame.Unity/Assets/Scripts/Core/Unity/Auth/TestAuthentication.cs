using Core.Basic;
using System.Threading.Tasks;

namespace Core.Unity.Auth
{
    internal class TestAuthentication : IAuthentication
    {
        public string UserId => "test-user-id";

        public string UserName => "test-user";

        public Task<string> GetToken()
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> SignIn()
        {
            throw new System.NotImplementedException();
        }
    }
}
