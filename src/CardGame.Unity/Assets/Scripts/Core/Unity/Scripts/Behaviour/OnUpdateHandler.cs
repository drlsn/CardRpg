using UnityEngine;
using UnityEngine.Events;

namespace Common.Unity.Scripts.Common
{
    public class OnUpdateHandler : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent _onUpdate;

        [SerializeField]
        private float _delaySeconds;

        [SerializeField]
        private bool _onlyOnce;

        private float _delaySecondsCurrent;
        private bool _alreadyDoneOnce;

        public void Init(UnityAction onUpdate, bool onlyOnce, float delaySeconds)
        {
            _onUpdate = new UnityEvent();
            _onUpdate.AddListener(onUpdate);
            _onlyOnce = onlyOnce;
            _delaySeconds = delaySeconds;

            _delaySecondsCurrent = 0;
            _alreadyDoneOnce = false;
        }

        private void OnDestroy()
        {
            _onUpdate?.RemoveAllListeners();
        }

        private void Update()
        {
            if (_delaySecondsCurrent >= _delaySeconds)
            {
                if (_onlyOnce)
                {
                    if (_alreadyDoneOnce)
                        return;

                    _onUpdate?.Invoke();
                    _alreadyDoneOnce = true;
                }
                else
                    _onUpdate?.Invoke();
            }

            _delaySecondsCurrent += Time.deltaTime;
        }
    }
}