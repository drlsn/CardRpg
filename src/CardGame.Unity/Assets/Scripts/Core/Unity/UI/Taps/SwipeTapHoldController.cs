using Core.Collections;
using Core.Unity.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Unity.UI.Taps
{
    [RequireComponent(typeof(MultiTapDetector))]
    [RequireComponent(typeof(HoldDetector))]
    [RequireComponent(typeof(SwipeDetector))]
    public class SwipeTapHoldController : UnityScript
    {
        private MultiTapDetector _tapDetector;
        private MultiTapDetector _tapDoubleDetector;
        private MultiTapDetector _tapTripleDetector;
        private HoldDetector _holdDetector;
        private SwipeDetector _swipeDetector;

        private bool _wasTap;
        private bool _wasDoubleTap;
        private bool _wasTripleTap;
        private bool _wasHold;
        private bool _wasSwipe;

        private bool _executedHold;
        private bool _executedSwipe;

        private readonly List<Action> _onTapHandlers = new();
        private readonly List<Action> _onSwipeHandlers = new();
        private readonly List<Action> _onHoldHandlers = new();

        private readonly List<Action> _toAddOnTapHandlers = new();
        private readonly List<Action> _toAddOnSwipeHandlers = new();
        private readonly List<Action> _toAddOnHoldHandlers = new();

        private bool _removeHandlers;

        private void Awake()
        {
            //var tapDetectors = Enumerable
            //    .Range(0, 3 - GetComponents<MultiTapDetector>().Length)
            //    .ForEach(i => gameObject.AddComponent<MultiTapDetector>())
            //    .ThenReturn(GetComponents<MultiTapDetector>());

            _tapDetector = GetComponent<MultiTapDetector>();// tapDetectors[0];
            //_tapDoubleDetector = tapDetectors[1];
            //_tapTripleDetector = tapDetectors[2];
            _holdDetector = GetComponent<HoldDetector>();
            _swipeDetector = GetComponent<SwipeDetector>();

            _tapDetector.OnDetected += OnTapDetected;
            //_tapDoubleDetector.OnDetected += OnTapDoubleDetected;
            //_tapTripleDetector.OnDetected += OnTapTripleDetected;
            _holdDetector.OnDetected += OnHoldDetected;
            _swipeDetector.OnDetected += OnSwipeDetected;
        }

        public void OnTap(Action handler) => _toAddOnTapHandlers.Add(handler);
        public void OnSwipe(Action handler) => _toAddOnSwipeHandlers.Add(handler);
        public void OnHold(Action handler) => _toAddOnHoldHandlers.Add(handler);

        private void OnTapDetected() => _wasTap = true;
        private void OnTapDoubleDetected() => _wasDoubleTap = true;
        private void OnTapTripleDetected() => _wasTripleTap = true;
        private void OnHoldDetected() => _wasHold = true;
        private void OnSwipeDetected() => _wasSwipe = true;

        private void OnDestroy()
        {
            _tapDetector.OnDetected -= OnTapDetected;
            //_tapDoubleDetector.OnDetected -= OnTapDoubleDetected;
            //_tapTripleDetector.OnDetected -= OnTapTripleDetected;
            _holdDetector.OnDetected -= OnHoldDetected;
            _swipeDetector.OnDetected -= OnSwipeDetected;
        }

        public void RemoveHandlers() => _removeHandlers = true;

        public void DisableAndRemoveHandlers()
        {
            RemoveHandlers();
            enabled = false;
        }

        private void Update()
        {
            if (_wasSwipe && !_wasHold)
            {
                if (!_executedSwipe)
                    _onSwipeHandlers.ForEachReversed(x => x());

                _executedSwipe = true;
            }

            if (_wasHold && !_wasSwipe)
            {
                if (!_executedHold)
                    _onHoldHandlers.ForEachReversed(x => x());

                _executedHold = true;
            }

            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended ||
                Input.GetMouseButtonUp(0))
            {
                if (!_wasHold && !_wasSwipe)
                {
                    if (_wasTap)
                        _onTapHandlers.ForEachReversed(x => x());
                }

                _wasTap = false;
                _wasDoubleTap = false;
                _wasTripleTap = false;
                _wasHold = false;
                _wasSwipe = false;

                _executedHold = false;
                _executedSwipe = false;
            }

            if (_removeHandlers) 
            {
                _onTapHandlers.Clear();
                _onSwipeHandlers.Clear();
                _onHoldHandlers.Clear();

                _removeHandlers = false;
            }

            _onTapHandlers.AddRange(_toAddOnTapHandlers);
            _onSwipeHandlers.AddRange(_toAddOnSwipeHandlers);
            _onHoldHandlers.AddRange(_toAddOnHoldHandlers);

            _toAddOnTapHandlers.Clear();
            _toAddOnSwipeHandlers.Clear();
            _toAddOnHoldHandlers.Clear();
        }
    }
}
