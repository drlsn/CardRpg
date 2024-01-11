using UnityEngine;
using static Core.Unity.UnityIOs;

namespace Core.Unity.Popups
{
    public class MessagesController : MonoBehaviour
    {
        [SerializeField] private TextTmpIOList _popupIO;

        public MessagesController Show(string message)
        {
            var popup = _popupIO.Object ?? _popupIO.Instantiate();
            
            popup.transform.SetSiblingIndex(0);
            popup.text = message;
            //popup.rectTransform.anchoredPosition = new(
            //    popup.rectTransform.anchoredPosition.
            //    popup.rectTransform.anchoredPosition.y);
            popup.rectTransform.sizeDelta = new(
               popup.transform.parent.Get<RectTransform>().rect.width,
               popup.rectTransform.rect.height);

            //UILayoutRebuilder.RebuildAll();

            return this;
        }
    }
}
