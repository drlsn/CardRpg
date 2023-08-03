using UnityEngine;
using UnityEngine.UI;

namespace Core.Unity.UI
{
    public class UILayoutRebuilder
    {
        public static void RebuildAll()
        {
            var transform = UnityEngine.GameObject.FindObjectOfType<Canvas>().GetComponent<RectTransform>();
            RefreshContentFitter(transform);
        }

        public static void Rebuild(UnityEngine.GameObject gameObject)
        {
            var transform = gameObject.GetComponent<RectTransform>();
            RefreshContentFitter(transform);
        }

        private static void RefreshContentFitter(RectTransform transform)
        {
            if (transform == null)
                return;

            foreach (RectTransform child in transform)
                RefreshContentFitter(child);

            bool isActive = transform.gameObject.activeSelf;
            transform.gameObject.SetActive(!isActive);
            transform.gameObject.SetActive(isActive);

            var layoutGroup = transform.GetComponent<LayoutGroup>();
            var contentSizeFitter = transform.GetComponent<ContentSizeFitter>();
            if (layoutGroup != null && contentSizeFitter != null)
            {
                contentSizeFitter.SetLayoutHorizontal();
                contentSizeFitter.SetLayoutVertical();
            }
            if (contentSizeFitter != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(transform);

            Canvas.ForceUpdateCanvases();
            transform.ForceUpdateRectTransforms();
        }
    }
}
