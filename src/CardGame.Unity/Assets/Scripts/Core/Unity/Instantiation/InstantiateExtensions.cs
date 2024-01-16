using Core.Unity.Math;
using UnityEngine;

namespace Core.Unity
{
    public static class InstantiateExtensions
    {
        public static T Instantiate<T>(this T prefab, Transform parent)
            where T : Component
        {
            if (prefab.transform is RectTransform rt)
            {
                var pos = rt.position.ToVector2() + (Vector2X.Half - rt.pivot) * rt.rect.size;
                return Object.Instantiate(prefab, pos, rt.rotation, parent).GetComponent<T>();
            }

            return Object.Instantiate(prefab, parent.ToTransform()).GetComponent<T>();
        }
    }
}
