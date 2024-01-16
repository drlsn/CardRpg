using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Unity.Coroutines
{
    public class CoroutineAggregate
    {
        private readonly Action<Coroutine> _stopCoroutine;
        private readonly List<Coroutine> _coroutines = new();

        public CoroutineAggregate(Action<Coroutine> stopCoroutine) =>
            _stopCoroutine = stopCoroutine;

        public void Stop() =>
            _coroutines.ForEach(_stopCoroutine);

        public static CoroutineAggregate operator +(CoroutineAggregate aggregate, Coroutine coroutine)
        {
            aggregate._coroutines.Add(coroutine);
            return aggregate;
        }
    }

    public static class CoroutineAggregateExtensions
    {
        public static CoroutineAggregate AddTo(this Coroutine coroutine, CoroutineAggregate aggregate) =>
            aggregate += coroutine;
    }
}
