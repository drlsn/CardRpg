using System.Collections.Generic;
using UnityEngine;

namespace Core.Unity.Transforms
{
    public static class TransformTranslationExtensions
    {
        public static Vector2 TranslateX(this Vector3 point, float translationX)
        {
            point.x += translationX;
            return new Vector2(point.x, point.y);
        }

        public static void TranslateX(this IEnumerable<Transform> transforms, float value)
        {
            foreach (var transform in transforms)
                transform.TranslateX(value);
        }

        public static void TranslateX(this Transform transform, float value)
        {
            var position = transform.position;
            position.x += value;
            transform.position = position;
        }

        public static void SetPosition(this Transform transform, Vector2 value)
        {
            var position = transform.position;
            position.x = value.x;
            position.y = value.y;
            transform.position = position;
        }

        public static void SetLocalPosition(this Transform transform, Vector2 value)
        {
            var position = transform.localPosition;
            position.x = value.x;
            position.y = value.y;
            transform.localPosition = position;
        }

        public static void SetLocalScale(this Transform transform, Vector2 value)
        {
            var source = transform.localScale;
            source.x = value.x;
            source.y = value.y;
            transform.localScale = source;
        }

        public static void SetRotationZ(this Transform transform, float value)
        {
            var source = transform.eulerAngles;
            source.z = value;
            transform.rotation = Quaternion.Euler(source);
        }

        public static void SetX(this Transform transform, float value)
        {
            var position = transform.position;
            position.x = value;
            transform.position = position;
        }

        public static void SetY(this Transform transform, float value)
        {
            var position = transform.position;
            position.y = value;
            transform.position = position;
        }

        public static void TranslateY(this IEnumerable<Transform> transforms, float value)
        {
            foreach (var transform in transforms)
                transform.TranslateY(value);
        }

        public static void TranslateY(this Transform transform, float value)
        {
            var position = transform.position;
            position.y += value;
            transform.position = position;
        }
    }
}
