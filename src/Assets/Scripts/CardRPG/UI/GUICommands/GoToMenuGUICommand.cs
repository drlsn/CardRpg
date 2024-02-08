using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardRPG.UI.GUICommands
{
    public class GoToMenuGUICommand : MonoBehaviour
    {
        public Task<bool> Execute()
        {
            SceneManager.LoadScene("Menu");
            return Task.FromResult(true);
        }
    }
}
