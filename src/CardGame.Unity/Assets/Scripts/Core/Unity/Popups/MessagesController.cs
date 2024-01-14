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

        public MessagesController Show(string message, float moveTime = 0.5f, float waitTime = 0.75f)
        {
            var popup = _popupIO.Object ?? _popupIO.Instantiate(new Vector2(-2000, 0));

            var rt = popup.RT();

            rt.SetSiblingIndex(0);
            popup.GetComponentsInChildren<TMP_Text>(includeInactive: true)
                .ForEach(text => text.text = message);

            LerpFunctions.LerpPosition2D(
                 StartCoroutine,
                 rt,
                 new Vector2(Screen.width / 2, Screen.height - 200),
                 durationSeconds: moveTime,
                 onDone: () =>
                 {
                     StartCoroutine(DestroyText(waitTime));
                 });

            return this;
        }

        private IEnumerator DestroyText(float waitTime = 0.75f)
        {
            yield return new WaitForSeconds(waitTime);

            var rt = _popupIO.Object.RT();
            var time = 0.5f;
            LerpFunctions.LerpPosition2D(
                 StartCoroutine,
                 rt,
                 new Vector2(5000, 0),
                 durationSeconds: time,
                 onDone: () =>
                 {
                     _popupIO.Destroy();
                 });

            LerpFunctions.LerpRotationZ(
                 StartCoroutine,
                 rt,
                 -360,
                 durationSeconds: time);
        }
    }
}
