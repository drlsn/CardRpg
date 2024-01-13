using UnityEngine;

namespace Core.Unity.UI
{
    public static class RectTransformPositionExtensions
    {
        public static void SetX(this RectTransform rt, float value) =>
            rt.anchoredPosition = new(value, rt.anchoredPosition.y);

        public static void SetY(this RectTransform rt, float value) =>
            rt.anchoredPosition = new(rt.anchoredPosition.x, value);
    }
}
