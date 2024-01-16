using Core.Unity.UI;
using System;
using UnityEngine;

namespace Core.Unity.Scripts.Transforms
{
    internal class DisplayRectTransformInfo : MonoBehaviour
    {
        [SerializeField] private RectInfo _screen = new();
        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector2 _anchoredPosition;
        [SerializeField] private Vector2 _pivot;
        [SerializeField] private RectInfo _rect = new();
        [SerializeField] private Vector2 _sizeDelta;
        [SerializeField] private Vector2 _lossyScale;

        private RectTransform _rt;

        private void Awake()
        {
            _rt = GetComponent<RectTransform>();
        }

        private void Update()
        {
            _screen = new RectInfo
            {
                Center = ScreenEx.Center,
                Size = ScreenEx.Size,

                Left = new Vector2(0, Screen.height / 2),
                Right = new Vector2(Screen.width, Screen.height / 2),
                Top = new Vector2(Screen.width / 2, Screen.height),
                Bottom = new Vector2(Screen.width / 2, 0),
                
                Pos = ScreenEx.Center,
            };

            _rect = new RectInfo
            {
                Center = _rt.rect.center,
                Size = _rt.rect.size,

                Left = new Vector2(_rt.rect.xMin, (_rt.rect.yMax - _rt.rect.yMin) / 2),
                Right = new Vector2(_rt.rect.xMax, (_rt.rect.yMax - _rt.rect.yMin) / 2),
                Top = new Vector2((_rt.rect.xMax - _rt.rect.xMin) / 2, _rt.rect.yMax),
                Bottom = new Vector2((_rt.rect.xMax - _rt.rect.xMin) / 2, _rt.rect.yMin),

                Pos = _rt.rect.position,
            };

            _position = _rt.position;
            _pivot = _rt.pivot;
            _anchoredPosition = _rt.anchoredPosition;
            _lossyScale = _rt.lossyScale;
        }
    }

    [Serializable]
    public class RectInfo
    {
        public Vector2 Pos;
        public Vector2 Center;
        
        public Vector2 Size;

        public Vector2 Left;
        public Vector2 Right;
        public Vector2 Top;
        public Vector2 Bottom;
    }
}
