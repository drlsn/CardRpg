using UnityEngine;

namespace Core.Unity.UI
{
    public static class RectTransformPositionExtensions
    {
        public static void SetX(this RectTransform rt, float value) =>
            rt.anchoredPosition = new(value, rt.anchoredPosition.y);

        public static void SetY(this RectTransform rt, float value) =>
            rt.anchoredPosition = new(rt.anchoredPosition.x, value);

        public static Vector2 GetPosFor(this RectTransform rt, RectTransform other)
        {
            return new Vector3(
                rt.position.x + rt.rect.width * (other.pivot.x - rt.pivot.x) * rt.lossyScale.x,
                rt.position.y + rt.rect.height * (other.pivot.y - rt.pivot.y) * rt.lossyScale.y,
                0);
        }
    }
}
