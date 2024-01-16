using UnityEngine;

namespace Core.Unity.Math
{
    public static class VectorExtensions
    {
        public static Vector2 AddX(this Vector2 vector, float value) =>
            new Vector2(vector.x + value, vector.y);

        public static Vector2 AddY(this Vector2 vector, float value) =>
            new Vector2(vector.x, vector.y + value);

        public static Vector3 AddX(this Vector3 vector, float value) =>
            new Vector2(vector.x + value, vector.y);

        public static Vector3 AddY(this Vector3 vector, float value) =>
            new Vector2(vector.x, vector.y + value);

        public static Vector3 Multiply(this Vector3 source, Vector3 other) =>
            new Vector3(
                source.x * other.x,
                source.y * other.y,
                source.z * other.z);

        public static Vector2 ToVector2(this Vector3 vector) => vector;
    }
}
