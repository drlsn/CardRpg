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

        public static void SetLeft(this Component rt, float left)
        {
            SetLeft(rt.GetComponent<RectTransform>(), left);
        }

        public static void SetRight(this Component rt, float right)
        {
            SetRight(rt.GetComponent<RectTransform>(), right);
        }

        public static void SetTop(this Component rt, float top)
        {
            SetTop(rt.GetComponent<RectTransform>(), top);
        }

        public static void SetBottom(this Component rt, float bottom)
        {
            SetBottom(rt.GetComponent<RectTransform>(), bottom);
        }

        public static void SetLeft(this RectTransform rt, float left)
        {
            rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        }

        public static void SetRight(this RectTransform rt, float right)
        {
            rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        }

        public static void SetTop(this RectTransform rt, float top)
        {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        }

        public static void SetBottom(this RectTransform rt, float bottom)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        }
    }
}
