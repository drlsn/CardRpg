using System.Collections.Generic;

namespace Core.Unity
{
    public interface IInstantiationObjectList<out T> : IInstantiationObject<T>
    {
        IEnumerable<T> Objects { get; }
        void Destroy(int index);
    }
}
