using System.Collections;
using TMPro;
using UnityEngine;
using static Core.Unity.UnityIOs;

namespace Core.Unity.Popups
{
    public class PopupController : MonoBehaviour
    {
        [SerializeField] private TextTmpIOList _popupIO;

        public void Show(string message)
        {
            var popup = _popupIO.Instantiate();
            
            popup.text = message;

            StartCoroutine(DestroyPopupAfterDelay(popup, 2f));
        }

        private IEnumerator DestroyPopupAfterDelay(
            TMP_Text text, float delay)
        {
            yield return new WaitForSeconds(delay);
            _popupIO.Destroy(text);
        }
    }
}
