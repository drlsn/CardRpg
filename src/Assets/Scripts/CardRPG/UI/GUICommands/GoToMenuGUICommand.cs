using CardRPG.UI.Features.IOs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardRPG.UI.GUICommands
{
    public class GoToMenuGUICommand : MonoBehaviour
    {
        [SerializeField] private BoardIO _boardIO;

        public void Execute()
        {
            _boardIO.Destroy();
            SceneManager.LoadScene(0);
        }
    }
}
