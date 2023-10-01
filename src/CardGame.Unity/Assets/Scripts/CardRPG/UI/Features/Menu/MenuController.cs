using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardRPG.UI.Menu
{
    public class MenuController : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene(1);
        }
    }
}
