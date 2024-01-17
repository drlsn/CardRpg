using UnityEngine;
using static Core.Unity.UnityIOs;

namespace Core.Unity.Scripts
{
    public class InstantiationObject : MonoBehaviour
    {
        [SerializeField] private TransformIO _prefabData;

        public void Instantiate()
        {
            _prefabData.Instantiate();
        }

        public void Destroy()
        {
            _prefabData.Destroy();
        }

        public TransformIO PrefabData => _prefabData;
    }
}
