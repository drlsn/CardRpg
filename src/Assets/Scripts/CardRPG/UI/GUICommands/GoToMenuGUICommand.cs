using CardRPG.UI.Features.Gameplay;
using CardRPG.UseCases.Users;
using Core.Net.Http;
using Corelibs.BlazorShared;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CardRPG.UI.GUICommands
{
    public class GoToMenuGUICommand : MonoBehaviour
    {
        [Inject] private IHttpClientAccessor _clientAccessor;

        public async Task<bool> Execute()
        {
            var result = await _clientAccessor.GetAsync<GetUserQueryResponse>(ClientType.TrinicaAuthorized, "api/v1/users/me");
            if (result is not null)
            {
                SceneManager.LoadScene("Menu");
                return true;
            }

            return false;
        }
    }
}
