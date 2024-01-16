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
                var instantiated = Object.Instantiate(prefab, parent.ToTransform()).GetComponent<T>();
                instantiated.GetComponent<RectTransform>().anchoredPosition = rt.anchoredPosition;

                return instantiated;
            }

            return Object.Instantiate(prefab, parent.ToTransform()).GetComponent<T>();
        }
    }
}
