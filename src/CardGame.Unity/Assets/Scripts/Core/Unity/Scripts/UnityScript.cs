using Common.Unity.Coroutines;
using System;
using UnityEngine;
using static Core.Unity.Functional.Delegates;

namespace Core.Unity.Scripts
{
    public class UnityScript : MonoBehaviour
    {
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

        protected Coroutine RunAsCoroutine(Action action) =>
            CoroutineExtensions.RunAsCoroutine(action, StartCoroutine);
    }
}
