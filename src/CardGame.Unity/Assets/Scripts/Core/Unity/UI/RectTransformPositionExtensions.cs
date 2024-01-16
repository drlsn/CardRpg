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

        public static Vector2 GetScreenPos(this RectTransform rt, float xOffset = 0, float yOffset = 0) =>
            new Vector2(
                rt.position.x + 
                    rt.rect.width * (0.5f - rt.pivot.x) * rt.lossyScale.x +
                    xOffset * rt.lossyScale.x,
                rt.position.y + 
                    rt.rect.height * (0.5f - rt.pivot.y) * rt.lossyScale.y +
                    yOffset * rt.lossyScale.y);
    }
}
