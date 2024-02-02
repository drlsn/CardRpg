using Common.Unity.Events;
using Core.Unity.Transforms;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Core.Maths.LerpingFunctions;

namespace Common.Unity.Scripts.Common
{
    public class LerpRotationZ : MonoBehaviour
    {
        [SerializeField] private GetGameObject _getTargetGO;
        [SerializeField] private Transform _target;
        [SerializeField] private float _targetValue;
        [SerializeField] private float _durationSeconds;
        [SerializeField] private List<UnityEvent> _onDone;
        [SerializeField] private LerpFunctionType _lerpFunctionType;

        public void Execute()
        {
            if (!_target)
                _target = _getTargetGO?.Invoke().transform;

            LerpFunctions.LerpRotationZ(
                StartCoroutine,
                _target, 
                _targetValue, 
                _durationSeconds, 
                _lerpFunctionType,
                onDone: () => _onDone.ForEach(h => h?.Invoke()));
        }
    }
}
