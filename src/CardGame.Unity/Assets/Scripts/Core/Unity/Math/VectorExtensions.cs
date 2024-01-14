using UnityEngine;

namespace Core.Unity.Math
{
    public static class VectorExtensions
    {
        public static Vector2 AddX(this Vector2 vector, float value) =>
            new Vector2(vector.x + value, vector.y);

        public static Vector2 AddY(this Vector2 vector, float value) =>
            new Vector2(vector.x, vector.y + value);
    }
}
