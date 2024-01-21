using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Unity.UI.Taps
{
    public class TapDetector : InputDetector, IPointerDownHandler
    {
        private float _lastTapTimeThreshold = 0.3f;
        private float _maxTimeBetweenTaps = 0.25f;
        private float _maxSwipeDistance = 10f;

        public Action OnDetected;

        private bool _isDown;
        private float _lastTapDownStartTime;
        private float _lastTapUpStartTime;

        private Vector2 _drag;
        private Vector2 _lastMousePos;

        private void Update()
        {
            if (!Interactable)
                return;

            if (IsPointerDown())
                OnPointerDown(null);

            if (Input.GetMouseButtonDown(0))
            {
                _lastMousePos = Input.mousePosition;
            }

            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                _drag += Input.GetTouch(0).deltaPosition;
            }
            else
            if (Input.GetMouseButton(0))
            {
                _drag += (Vector2) Input.mousePosition - _lastMousePos;
                _lastMousePos = Input.mousePosition;
            }

            if (_drag.magnitude > _maxSwipeDistance)
            {
                Reset();
                return;
            }

            if (IsPointerUp())
            {
                if (Time.time < _lastTapDownStartTime + _maxTimeBetweenTaps)
                {
                    Reset();

                    OnDetected?.Invoke();
                }
            }
        }

        private void Reset()
        {
            _lastTapUpStartTime = 0;
            _lastTapDownStartTime = 0;
            _isDown = true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isDown = true;
            _lastTapDownStartTime = Time.time;
            _drag = Vector2.zero;
        }
    }
}
