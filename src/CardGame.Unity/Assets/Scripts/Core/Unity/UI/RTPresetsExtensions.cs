using UnityEngine;

namespace Core.Unity.UI
{
    public static class RTPresetsExtensions
    {
        #region Component

        public static void StretchToExtents(this Component rt) => rt.GetComponent<RectTransform>().StretchToExtents();
        public static void StretchToMiddle(this Component rt) => rt.GetComponent<RectTransform>().StretchToMiddle();
        public static void StretchToTop(this Component rt) => rt.GetComponent<RectTransform>().StretchToTop();

        #endregion

        #region RectTransform

        public static void StretchToExtents(this RectTransform rt)
        {
            var rtParent = rt.parent.GetComponent<RectTransform>();

            rt.anchoredPosition = rtParent.position;

            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);

            rt.offsetMin = new Vector2(0, 0);
            rt.offsetMax = new Vector2(0, 0);

            rt.pivot = new Vector2(0.5f, 0.5f);
        }

        public static void StretchToMiddle(this RectTransform rt)
        {
            var rtParent = rt.parent.GetComponent<RectTransform>();

            rt.anchoredPosition = new Vector2(0.0f, 0.0f);

            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);

            rt.SetWidth(rtParent.sizeDelta.x);
            rt.SetHeight(rtParent.sizeDelta.y);

            rt.pivot = new Vector2(0.5f, 0.5f);
        }

        public static void StretchToTop(this RectTransform rt)
        {
            rt.pivot = new Vector2(0.5f, 1f);

            rt.anchorMin = new Vector2(0.0f, 1.0f);
            rt.anchorMax = new Vector2(1.0f, 1.0f);

            rt.SetY(0);

            rt.SetLeft(0);
            rt.SetRight(0);
        }

        public static void StretchToBottom(this RectTransform rt)
        {
            rt.pivot = new Vector2(0.5f, 0f);

            rt.anchorMin = new Vector2(0.0f, 0.0f);
            rt.anchorMax = new Vector2(1.0f, 0.0f);

            rt.SetY(0);

            rt.SetLeft(0);
            rt.SetRight(0);
        }

        #endregion
    }
}
