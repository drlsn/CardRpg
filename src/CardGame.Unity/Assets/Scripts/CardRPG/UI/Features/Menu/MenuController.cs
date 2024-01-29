using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using static CardRPG.UI.Features.Gameplay.DependencyInstaller;

namespace CardRPG.UI.Menu
{
    public class MenuController : MonoBehaviour
    {
        [Inject]
        public Greeter Greeter { get; set; }

        public void Play()
        {
            Debug.Log(Greeter);
            SceneManager.LoadScene("Gameplay");
        }
    }
}
