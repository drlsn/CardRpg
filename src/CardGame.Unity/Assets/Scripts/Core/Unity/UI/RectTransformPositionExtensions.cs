using UnityEngine;

namespace Core.Unity.UI
{
    public static class RectTransformPositionExtensions
    {
        public static void SetX(this RectTransform rt, float value) =>
            rt.anchoredPosition = new(value, rt.anchoredPosition.y);

        public static void SetY(this RectTransform rt, float value) =>
            rt.anchoredPosition = new(rt.anchoredPosition.x, value);

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
