using Common.Unity.Coroutines;
using Core.Collections;
using Core.Unity.Popups;
using System;
using UnityEngine;
using static Core.Unity.Functional.Delegates;

namespace Core.Unity.Scripts
{
    public class UnityScript : MonoBehaviour
    {
        private MessagesController _msg;

        private void Awake()
        {
            _msg = GameObject.FindObjectOfType<MessagesController>();
            OnAwake();
        }

        protected virtual void OnAwake() {}

        protected Callback Run(Action action, float delaySeconds = 0f) =>
            onDone =>
            {
                if (delaySeconds <= 0f)
                {
                    action();
                    onDone();
                    return;
                }

                CoroutineExtensions.RunAsCoroutine(() =>
                {
                    action();
                    onDone();
                },
                delaySeconds,
                StartCoroutine);
            };

        protected Callback Wait(float delaySeconds = 0.5f) =>
            Run(() => { }, delaySeconds);

        protected Coroutine RunAsCoroutine(Action action, float delaySeconds = 0f) =>
            CoroutineExtensions.RunAsCoroutine(action, delaySeconds, StartCoroutine);


        protected Callback Show(string message, float showTimeSeconds = int.MaxValue) => onDone =>
        {
            _msg.Show(message, showTimeSeconds, false);
            onDone?.Invoke();
        };

        protected void Log(string message) => Debug.Log(message);
        protected void LogScriptName() => Debug.Log(GetType().Name);
    }
}

public static class LogExtensions
{
    public static void Log(this object x, string name = "")
    {
        if (name.IsNullOrEmpty())
            Debug.Log(x);
        else
            Debug.Log($"{name}: {x}");
    }
}
