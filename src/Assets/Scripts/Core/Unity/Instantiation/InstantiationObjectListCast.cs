using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Unity
{
    public class InstantiationObjectListCast<T> : InstantiationObjectCast<T>, IInstantiationObjectList<T>
    {
        private readonly IInstantiationObjectList<Component> _anyComponentIO;

        public InstantiationObjectListCast(IInstantiationObjectList<Component> anyComponentIO)
            : base(anyComponentIO)
        {
            _anyComponentIO = anyComponentIO;
        }

        public IEnumerable<T> Objects => _anyComponentIO.Objects.Select(c => c.GetComponent<T>());

        public void Destroy(int index, int count = 1) => _anyComponentIO.Destroy(index, count);

    }
}
