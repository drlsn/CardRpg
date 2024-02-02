using UnityEngine;

namespace Core.Unity
{
    public interface IInstantiationObject<out T>
    {
        T Instantiate();

        void Destroy();

        T Object { get; }
        Transform Parent { get; }
        T Prefab { get; }
    }
}
