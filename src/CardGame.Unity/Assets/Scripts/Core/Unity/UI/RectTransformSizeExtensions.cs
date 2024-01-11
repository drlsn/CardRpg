using UnityEngine;

namespace Core.Unity.UI
{
    public static class RectTransformSizeExtensions
    {
        public static void SetWidth(this RectTransform rt, float value) =>
            rt.sizeDelta = new(value, rt.sizeDelta.y);

        public static void SetHeight(this RectTransform rt, float value) =>
            rt.sizeDelta = new(rt.sizeDelta.x, value);

        public static void SetSize(this RectTransform rt, float x, float y) =>
            rt.sizeDelta = new(x, y);

        public static void SetSize(this RectTransform rt, Vector2 value) =>
            rt.SetSize(value.x, value.y);

        public static float GetRTWidth(this Transform transform) =>
           transform.Get<RectTransform>().rect.width;

        public static float GetRTHeight(this Transform transform) =>
            transform.Get<RectTransform>().rect.height;

        public static float GetParentRTWidth(this Transform transform) =>
            transform.parent.GetRTWidth();

        public static float GetParentRTHeight(this Transform transform) =>
            transform.parent.GetRTHeight();
    }
}
