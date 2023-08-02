using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Core.Unity.UnityIOs;

namespace Core.Unity.Scripts
{
    public class InstantiationObjectList : MonoBehaviour, IInstantiationObjectList<Transform>
    {
        public int ObjectCount;
        [SerializeField] private TransformIOList _data;

        public Transform Object => _data.Object;
        public IEnumerable<Transform> Objects => _data.Objects;

        public Transform Parent => _data.Parent;

        public Transform Prefab => _data.Prefab;

        public Transform Instantiate()
        {
            var obj = _data.Instantiate();

            ObjectCount = Objects.Count();

            return obj;
        }

        public void Destroy()
        {
            _data.Destroy();
        }

        public void Destroy(int index)
        {
            _data.Destroy(index);
        }
    }
}
