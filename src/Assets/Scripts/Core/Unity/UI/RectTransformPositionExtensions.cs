using Core.Unity.Math;
using UnityEngine;

namespace Core.Unity.UI
{
    public static class RectTransformPositionExtensions
    {
        public static void SetAnchoredPosX(this RectTransform rt, float value) =>
            rt.anchoredPosition = new(value, rt.anchoredPosition.y);

        public static void SetAnchoredPosY(this RectTransform rt, float value) =>
            rt.anchoredPosition = new(rt.anchoredPosition.x, value);

        public static void AddAnchoredPosX(this RectTransform rt, float value) =>
            rt.anchoredPosition = new(rt.anchoredPosition.x + value, rt.anchoredPosition.y);

        public static void AddAnchoredPosY(this RectTransform rt, float value) =>
            rt.anchoredPosition = new(rt.anchoredPosition.x, rt.anchoredPosition.y + value);

        public static void TranslateByWidth(this RectTransform rt) =>
            rt.AddAnchoredPosX(rt.rect.width);

        public static void TranslateByWidthHalf(this RectTransform rt) =>
            rt.AddAnchoredPosX(rt.rect.width / 2);

        public static Vector2 GetScreenCenterPos(this RectTransform rt, float xOffset = 0, float yOffset = 0) =>
            new Vector2(
                rt.position.x + 
                    rt.rect.width * (0.5f - rt.pivot.x) * rt.lossyScale.x +
                    xOffset * rt.lossyScale.x,
                rt.position.y + 
                    rt.rect.height * (0.5f - rt.pivot.y) * rt.lossyScale.y +
                    yOffset * rt.lossyScale.y);


        public static bool IsInRect(this Vector3 position, RectTransform rt) =>
            IsInRect(position.ToVector2(), rt);

        public static bool IsInRect(this Vector2 position, RectTransform rt)
        {
            var left = rt.GetScreenCenterPos(xOffset: -rt.GetRTWidth() / 2);
            var right = rt.GetScreenCenterPos(xOffset: rt.GetRTWidth() / 2);
            var top = rt.GetScreenCenterPos(yOffset: rt.GetRTHeight() / 2);
            var bottom = rt.GetScreenCenterPos(yOffset: -rt.GetRTHeight() / 2);

            return 
                position.x > left.x && position.x < right.x &&
                position.y > bottom.y && position.y < top.y;
        }
    }
}
