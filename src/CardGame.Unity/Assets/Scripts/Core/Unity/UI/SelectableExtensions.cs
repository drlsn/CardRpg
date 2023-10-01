using Core.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace Core.Unity.UI
{
    public static class SelectableExtensions
    {
        public static void SetInteractable(this object objectWithSelectables, bool value) =>
            SetInteractable(new[] { objectWithSelectables }, value);

        public static void SetInteractable(this IEnumerable<object> objectsWithSelectables, bool value)
        {
            var items = objectsWithSelectables.GetThisAndNestedChildren<Selectable>().ToList();
            items.SetInteractable(value);
        }

        public static void SetInteractable(this IEnumerable<Selectable> selectables, bool value)
        {
            selectables.ForEach(s => s.interactable = value);
        }
    }
}
