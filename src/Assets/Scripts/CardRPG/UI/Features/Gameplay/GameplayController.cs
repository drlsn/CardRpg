using CardRPG.UI.GUICommands;
using System.Threading.Tasks;
using UnityEngine;

namespace CardRPG.UI.Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        private async void Start()
        {
            await Play();
        }

        public async Task Play()
        {
            Application.targetFrameRate = 60;
            await GameObject.FindObjectOfType<StartGameGUICommand>()
                .Execute();
        }
    }
}
