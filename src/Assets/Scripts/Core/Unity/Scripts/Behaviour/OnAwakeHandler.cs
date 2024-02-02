using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Common.Unity.Scripts.Common
{
    public class OnAwakeHandler : MonoBehaviour
    {
        [SerializeField] private List<UnityEvent> _onAwake;

        private void Awake()
        {
            _onAwake?.ForEach(h => h.Invoke());
        }
    }
}