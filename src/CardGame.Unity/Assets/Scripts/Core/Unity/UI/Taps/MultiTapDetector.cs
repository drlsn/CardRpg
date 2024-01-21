using Core.Unity.Scripts;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Unity.UI.Taps
{
    public class MultiTapDetector : InputDetector
    {
        [SerializeField] private float _lastTapTimeThreshold = 0.3f;
        [SerializeField] private float _maxTimeBetweenTaps = 0.25f;

        public int Taps = 2;

        public event Action OnDetected;

        private int _tapCount;
        private float _lastTapDownStartTime;
        private float _lastTapUpStartTime;

        private void Update()
        {
            if (!Interactable)
                return;

            if (IsPointerDown())
                OnPointerDown(null);

            if (IsPointerUp())
                OnPointerUp(null);

            if (_lastTapUpStartTime == 0)
                return;

            if ((Taps == 1 && _tapCount == 1 && Time.time < _lastTapDownStartTime + _maxTimeBetweenTaps) ||
                (Taps > 1 && _tapCount == Taps && Time.time < _lastTapUpStartTime + _maxTimeBetweenTaps))
            {
                _lastTapUpStartTime = 0;
                _lastTapDownStartTime = 0;
                _tapCount = 0;

                OnDetected?.Invoke();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _lastTapDownStartTime = Time.time;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            var currentTime = Time.time;
            if (currentTime <= _lastTapUpStartTime + _maxTimeBetweenTaps)
                _tapCount++;
            else
                _tapCount = 1;

            _lastTapUpStartTime = Time.time;
        }
    }
}
