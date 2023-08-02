using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Unity
{ 
    public static class ViewToTransformConverter
    {
        public static IEnumerable<Transform> ToTransform(this IEnumerable<object> objects) => objects.Select(obj => obj.ToTransform());

        public static Transform ToTransform(this object @object)
        {
            if (@object is Component component)
                return component.transform;

            if (@object is GameObject gameObject)
                return gameObject.transform;

            return null;
        }
    }
}
