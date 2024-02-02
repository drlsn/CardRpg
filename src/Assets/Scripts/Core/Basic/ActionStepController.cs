using Core.Collections;
using Core.Functional;
using System;
using System.Collections.Generic;
using static Core.Unity.Functional.Delegates;

namespace Core.Basic
{
    public class ActionStepController
    {
        private List<Callback> _callbacks = new();
        private int _index;

        public static ActionStepController operator +(ActionStepController left, Callback right) =>
            right
                .AddTo(left._callbacks)
                .ThenReturn(left);

        public void Execute(Action onDone)
        {
            _index = 0;
            ExecuteNextCallback(onDone);
        }

        public void Execute() => Execute(null);

        private void ExecuteNextCallback(Action onDone)
        {
            if (_index < _callbacks.Count)
            {
                var currentCallback = _callbacks[_index];

                Action onDoneLocal = () =>
                {
                    _index++;
                    ExecuteNextCallback(onDone);

                    if (_index == _callbacks.Count)
                        onDone?.Invoke();
                };

                currentCallback(onDoneLocal);
            }
        }
    }
}
