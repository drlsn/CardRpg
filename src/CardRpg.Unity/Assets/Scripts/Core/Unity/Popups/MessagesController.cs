using Core.Unity.UI;
using UnityEngine;
using static Core.Unity.UnityIOs;

namespace Core.Unity.Popups
{
    public class MessagesController : MonoBehaviour
    {
        [SerializeField] private TextTmpIOList _popupIO;

        public MessagesController Show(string message)
        {
            var popup = _popupIO.Instantiate();
            popup.transform.SetSiblingIndex(0);
            popup.text = message;

            UILayoutRebuilder.RebuildAll();

            return this;
        }
    }
}
