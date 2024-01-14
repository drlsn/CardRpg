using Core.Collections;
using Core.Unity.Transforms;
using Core.Unity.UI;
using System.Collections;
using TMPro;
using UnityEngine;
using static Core.Unity.UnityIOs;

namespace Core.Unity.Popups
{
    public class MessagesController : MonoBehaviour
    {
        [SerializeField] private TextTmpIOList _popupIO;

        public MessagesController Show(string message, float showTime = 0.25f, float waitTime = 0f)
        {
            var popup = _popupIO.Object ?? _popupIO.Instantiate();

            var rt = popup.RT();
            rt.localScale = Vector3.zero;

            rt.SetSiblingIndex(0);
            popup.GetComponentsInChildren<TMP_Text>(includeInactive: true)
                .ForEach(text => text.text = message);

            LerpFunctions.LerpScale2D(
                    StartCoroutine,
                    rt,
                    1,
                    durationSeconds: showTime,
                    onDone: () =>
                    {
                        if (waitTime != 0)
                            StartCoroutine(DestroyText(waitTime));
                    });

            return this;
        }

        private bool _explicitDestroy;

        public void HideMessage()
        {
            _explicitDestroy = true;
            StartCoroutine(DestroyText(0));
        }

        private IEnumerator DestroyText(float waitTime = 0.75f)
        {
            yield return new WaitForSeconds(waitTime);

            if (_explicitDestroy)
                yield return null;

            var rt = _popupIO.Object.RT();
            var time = 0.5f;
            LerpFunctions.LerpScale2D(
                 StartCoroutine,
                 rt,
                 0,
                 durationSeconds: time,
                 onDone: () =>
                 {
                     _popupIO.Destroy();
                     _explicitDestroy = false;
                 });
        }
    }
}
