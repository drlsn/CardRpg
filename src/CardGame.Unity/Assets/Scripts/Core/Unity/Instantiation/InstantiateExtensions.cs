using UnityEngine;

namespace Core.Unity
{
    public static class InstantiateExtensions
    {
        public static TObject Instantiate<TObject, TParent>(this Component prefab, TParent parent) 
            => GameObject.Instantiate(prefab, parent.ToTransform()).GetComponent<TObject>();
    }

}
