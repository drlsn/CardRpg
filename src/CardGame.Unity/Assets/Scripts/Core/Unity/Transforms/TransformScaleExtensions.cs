using System.Collections.Generic;
using UnityEngine;

namespace Core.Unity.Transforms
{
    public static class TransformScaleExtensions
    {
        public static void ScalePositiveX(this IEnumerable<Transform> transforms)
        {
            foreach (var transform in transforms)
                transform.ScalePositiveX();
        }

        public static void ScalePositiveX(this Transform transform)
        {
            var scale = transform.localScale;
            scale.x = System.Math.Abs(scale.x);
            transform.localScale = scale;
        }

        public static void ScaleNegativeX(this IEnumerable<Transform> transforms)
        {
            foreach (var transform in transforms)
                transform.ScaleNegativeX();
        }

        public static void ScaleNegativeX(this Transform transform)
        {
            var scale = transform.localScale;
            scale.x = -System.Math.Abs(scale.x);
            transform.localScale = scale;
        }

        public static void ScaleReverseX(this IEnumerable<Transform> transforms)
        {
            foreach (var transform in transforms)
                transform.ScaleReverseX();
        }

        public static void ScaleReverseX(this Transform transform)
        {
            var scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
        }

        public static void ScaleSetX(this IEnumerable<Transform> transforms, float value)
        {
            foreach (var transform in transforms)
                transform.ScaleSetX(value);
        }

        public static void ScaleSetX(this Transform transform, float value)
        {
            var scale = transform.localScale;
            scale.x = value;
            transform.localScale = scale;
        }

        public static void ScaleX(this IEnumerable<Transform> transforms, float value)
        {
            foreach (var transform in transforms)
                transform.ScaleX(value);
        }

        public static void ScaleX(this Transform transform, float value)
        {
            var scale = transform.localScale;
            scale.x *= value;
            transform.localScale = scale;
        }

        public static void ScaleY(this IEnumerable<Transform> transforms, float value)
        {
            foreach (var transform in transforms)
                transform.ScaleY(value);
        }

        public static void ScaleY(this Transform transform, float value)
        {
            var scale = transform.localScale;
            scale.y *= value;
            transform.localScale = scale;
        }
    }
}
