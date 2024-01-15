using Core.Collections;
using Core.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Unity.Components
{
    public static class GraphicExtensions
    {
        public static void NestedGraphicsToClear(this IEnumerable<GameObject> gameObjects)
        {
            gameObjects.ForEach(go => go.NestedGraphicsToClear());
        }

        public static void NestedGraphicsToClear(this GameObject gameObject)
        {
            gameObject.ForEachNested<Graphic>(g => g.ToClearSingle());
        }

        public static void NestedGraphicsToTransparent(this IEnumerable<GameObject> gameObjects)
        {
            gameObjects.ForEach(go => go.NestedGraphicsToTransparent());
        }

        public static void NestedGraphicsToTransparent(this GameObject gameObject)
        {
            gameObject.ForEachNested<Graphic>(g => g.ToTransparentSingle());
        }

        public static void ToTransparentSingle(this Graphic graphic)
        {
            graphic.ModifyChannel(3, 0);
        }

        public static void ToClearSingle(this Graphic graphic)
        {
            graphic.ModifyChannel(3, 1);
        }

        public static void ModifyChannel(this Graphic graphic, int index, float value)
        {
            var color = graphic.color;
            color[index] = value;
            graphic.color = color;
        }

        public static IEnumerable<Component> GetVisibleChildren(this Component component)
            => component.GetChildren<Graphic>().Where(g => g.color.a > 0);

        public static void SetSprite(this Component component, Sprite sprite)
        {
            var image = component.GetComponent<Image>();
            if (!image)
                return;

            image.sprite = sprite;
        }
    }
}
