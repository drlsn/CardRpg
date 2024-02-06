using UnityEngine;

namespace Core.Unity.Scripts.Behaviour
{
    internal class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
