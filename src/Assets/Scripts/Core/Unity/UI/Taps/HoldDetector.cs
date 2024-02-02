using Core.Unity.Scripts;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Unity.UI.Taps
{
    public class HoldDetector : UnityScript, IPointerDownHandler
    {
        [SerializeField] private float _holdTime = 0.6f;

        private long _tapStartTime;

        public event Action OnDetected;

        private void Update()
        {
            if (_tapStartTime == 0)
                return;

            if (DateTime.UtcNow.Ticks >= (_tapStartTime + (long) (_holdTime * TimeSpan.TicksPerSecond)))
            {
                _tapStartTime = 0;
                OnDetected?.Invoke();
            }

            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended ||
                Input.GetMouseButtonUp(0))
            {
                _tapStartTime = 0;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _tapStartTime = DateTime.UtcNow.Ticks;
        }
    }
}
