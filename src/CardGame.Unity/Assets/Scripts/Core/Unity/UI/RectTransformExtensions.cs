using UnityEngine;

namespace Core.Unity.UI
{
    public static class RectTransformExtensions
    {
        public static RectTransform RT(this Component component) =>
            component.transform as RectTransform;

        public static RectTransform RT(this GameObject go) =>
            go.transform.RT();
    }
}
