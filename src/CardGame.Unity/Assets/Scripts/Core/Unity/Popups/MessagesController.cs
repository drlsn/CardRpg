using Common.Unity.Coroutines;
using Core.Collections;
using Core.Unity.Coroutines;
using Core.Unity.Transforms;
using Core.Unity.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Core.Unity.Functional.Delegates;
using static Core.Unity.UnityIOs;

namespace Core.Unity.Popups
{
    public class MessagesController : MonoBehaviour
    {
        public const float MinShowTime = 0.25f;
        public const float ShowTime = 0.25f;
        public const float StayTime = 0f;

        [SerializeField] private TextTmpIOList _popupIO;
        private RectTransform _msgRT;

        private readonly Queue<MessageInfo> _messages = new();
        private CoroutineAggregate _coroutines;

        private MessageState _messageState;
        private float _lastMsgTimeSeconds = int.MaxValue;

        private void Awake()
        {
            _coroutines = new(StopCoroutine);
            _popupIO.Instantiate();
            _popupIO.Object.RT().localScale = Vector3.zero;
            _msgRT = _popupIO.Object.RT();
        }

        public MessagesController Show(
            string message, float showTimeSeconds = int.MaxValue, bool force = false)
        {
            _messages.Enqueue(
                new(message, showTimeSeconds));
            
            return this;
        }

        private void Update()
        {
            if (_messages.Count == 0)
                return;

            if (_lastMsgTimeSeconds != int.MaxValue)
                return;

            var msg = _messages.Peek();

            if (_messageState == MessageState.Inactive)
            {
                _coroutines.Stop();
                _coroutines += FadeInText(msg.Text, msg.Time);
                _messages.Dequeue();
            }
            else 
            if (_messageState == MessageState.Showing)
            {
                _coroutines.Stop();
                _coroutines += FadeOutText();
            }
        }

        public void HideMessage(bool immediate = false)
        {
            _coroutines.Stop();

            if (immediate)
            {
                _msgRT.localScale = Vector3.zero;
                _messageState = MessageState.Inactive;
                _coroutines.Stop();

                return;
            }

            _coroutines.Stop();
            FadeOutText();
        }

        public Coroutine FadeInText(
            string message,
            float stayTimeSeconds)
        {
            _msgRT.SetSiblingIndex(0);
            _msgRT.GetComponentsInChildren<TMP_Text>(includeInactive: true)
                .ForEach(text => text.text = message);

            _lastMsgTimeSeconds = stayTimeSeconds;
            _messageState = MessageState.FadeIn;
            return LerpFunctions.LerpScale2D(
                StartCoroutine,
                _msgRT,
                1,
                durationSeconds: ShowTime,
                onDone: () =>
                {
                    _messageState = MessageState.Showing;
                    if (stayTimeSeconds != int.MaxValue)
                        CoroutineExtensions.RunAsCoroutine(() => FadeOutText(), delaySeconds: stayTimeSeconds, StartCoroutine);
                });
        }

        private Coroutine FadeOutText()
        {
            _messageState = MessageState.FadeOut;
            return LerpFunctions.LerpScale2D(
                StartCoroutine,
                _msgRT,
                0,
                durationSeconds: ShowTime,
                onDone: () =>
                {
                    _lastMsgTimeSeconds = int.MaxValue;
                    _messageState = MessageState.Inactive;
                });
        }

        private class MessageInfo
        {
            public readonly string Text = string.Empty;
            public readonly float Time = float.MaxValue;

            public MessageInfo(string text, float time = float.MaxValue)
            {
                Text = text;
                Time = time;
            }
        }

        private enum MessageState
        {
            Inactive,
            FadeIn,
            Showing,
            FadeOut
        }
    }
}
