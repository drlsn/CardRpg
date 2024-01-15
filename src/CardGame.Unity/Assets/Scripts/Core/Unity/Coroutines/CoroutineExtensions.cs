using System;
using System.Collections;
using UnityEngine;

namespace Common.Unity.Coroutines
{
    public static class CoroutineExtensions
    {
        public static Coroutine RunAsCoroutine(this Action action, Func<IEnumerator, Coroutine> startCoroutine)
        {
            return startCoroutine(RunAsCoroutineDelayed(0, action));
        }

        public static Coroutine RunAsCoroutine(this Action action, float delaySeconds, Func<IEnumerator, Coroutine> startCoroutine)
        {
            return startCoroutine(RunAsCoroutineDelayed(delaySeconds, action));
        }

        public static Coroutine RunAsCoroutineRepeated(this Action action, float repeatSeconds, Func<IEnumerator, Coroutine> startCoroutine)
        {
            return startCoroutine(RunAsCoroutineRepeated(() => repeatSeconds, action));
        }

        public static Coroutine RunAsCoroutineRepeated(this Action action, Func<float> getRepeatSeconds, Func<IEnumerator, Coroutine> startCoroutine)
        {
            return startCoroutine(RunAsCoroutineRepeated(getRepeatSeconds, action));
        }

        private static IEnumerator RunAsCoroutineDelayed(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            action();
        }

        private static IEnumerator RunAsCoroutineRepeated(Func<float> getSeconds, Action action)
        {
            while (true)
            {
                action();
                yield return new WaitForSeconds(getSeconds());
            }
        }
    }
}
