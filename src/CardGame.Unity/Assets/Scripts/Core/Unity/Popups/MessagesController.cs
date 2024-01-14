using Core.Collections;
using Core.Unity.Transforms;
using Core.Unity.UI;
using System.Collections;
using TMPro;
using UnityEngine;
using static Core.Maths.LerpingFunctions;
using static Core.Unity.UnityIOs;

namespace Core.Unity.Popups
{
    public class MessagesController : MonoBehaviour
    {
        [SerializeField] private TextTmpIOList _popupIO;

        public MessagesController Show(string message)
        {
            var popup = _popupIO.Object ?? _popupIO.Instantiate(new Vector2(-2000, 0));

            var rt = popup.RT();

            rt.SetSiblingIndex(0);
            popup.GetComponentsInChildren<TMP_Text>(includeInactive: true)
                .ForEach(text => text.text = message);

            var time = 0.5f;
            LerpFunctions.LerpPosition2D(
                 rt,
                 new Vector2(Screen.width / 2, Screen.height - 200),
                 durationSeconds: time,
                 LerpFunctionType.Smooth,
                 StartCoroutine,
                 onDone: () =>
                 {
                     StartCoroutine(DestroyText());
                 });

            LerpFunctions.LerpRotationZ(
                 rt,
                 360,
                 durationSeconds: time,
                 LerpFunctionType.Smooth,
                 StartCoroutine,
                 onDone: () => {});

            return this;
        }

        private IEnumerator DestroyText()
        {
            yield return new WaitForSeconds(1f);

            var rt = _popupIO.Object.RT();
            var time = 0.5f;
            LerpFunctions.LerpPosition2D(
                 rt,
                 new Vector2(5000, 0),
                 durationSeconds: time,
                 LerpFunctionType.Smooth,
                 StartCoroutine,
                 onDone: () =>
                 {
                     _popupIO.Destroy();
                 });

            LerpFunctions.LerpRotationZ(
                 rt,
                 -360,
                 durationSeconds: time,
                 LerpFunctionType.Smooth,
                 StartCoroutine,
                 onDone: () => { });
        }
    }
}
