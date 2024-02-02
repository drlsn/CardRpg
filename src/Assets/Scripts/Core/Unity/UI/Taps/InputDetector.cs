using Core.Unity.Scripts;
using UnityEngine;

namespace Core.Unity.UI.Taps
{
    public abstract class InputDetector : UnityScript
    {
        public bool Interactable { get; set; } = true;

        protected RectTransform _rt;

        private void Awake()
        {
            _rt = this.RT();
        }

        protected bool IsPointerDown() =>
            (Input.GetMouseButtonDown(0) || Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) && 
            Input.mousePosition.IsInRect(_rt);

        protected bool IsPointerUp() =>
            (Input.GetMouseButtonUp(0) || Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
            && Input.mousePosition.IsInRect(_rt);
    }
}
