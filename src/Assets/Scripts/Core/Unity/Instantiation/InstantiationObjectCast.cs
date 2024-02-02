using UnityEngine;

namespace Core.Unity
{
    public class InstantiationObjectCast<T> : IInstantiationObject<T>
    {
        private readonly IInstantiationObject<Component> _anyComponentIO;

        public InstantiationObjectCast(IInstantiationObject<Component> anyComponentIO)
        {
            _anyComponentIO = anyComponentIO;
        }

        public T Object => _anyComponentIO.Object.GetComponent<T>();

        public Transform Parent => _anyComponentIO.Parent;

        public T Prefab => _anyComponentIO.Prefab.GetComponent<T>();

        public void Destroy()
        {
            _anyComponentIO.Destroy();
        }

        public T Instantiate()
        {
            return _anyComponentIO.Instantiate().GetComponent<T>();
        }
    }
}
