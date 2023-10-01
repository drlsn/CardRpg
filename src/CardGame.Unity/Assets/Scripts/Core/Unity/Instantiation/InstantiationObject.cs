using UnityEngine;

namespace Core.Unity
{
    public class InstantiationObject<T> : IInstantiationObject<T>
        where T : Component
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private T _prefab;
        private T _instantiated;

        public T Instantiate()
        {
            if (_parent && _parent.gameObject)
            {
                if (!_parent.gameObject.activeSelf)
                    _parent.gameObject.SetActive(true);
            }

            _instantiated = GameObject.Instantiate(_prefab, _parent);
            return _instantiated;
        }

        public void Destroy()
        {
            _instantiated.Destroy();
            _instantiated = null;
        }

        public T Object => _instantiated;
        public Transform Parent => _parent;
        public T Prefab => _prefab;
    }
}
