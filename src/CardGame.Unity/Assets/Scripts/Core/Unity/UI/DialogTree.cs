using Core.Collections;
using Core.Unity.Transforms;
using Core.Unity.UI.Taps;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Unity.UI
{
    public class DialogTree
    {
        private readonly Transform _view;
        private readonly Func<IEnumerator, Coroutine> _startCoroutine;

        private readonly Image _dialogParentBg;

        private readonly Stack<DialogInfo> _stack = new();

        public DialogTree(
            Transform view,
            Image bgPrefab,
            Func<IEnumerator, Coroutine> startCoroutine) 
        {
            _view = view;
            _startCoroutine = startCoroutine;

            _dialogParentBg = bgPrefab.Instantiate(view);
            _dialogParentBg.transform.position = ScreenEx.Center;
            _dialogParentBg.StretchToExtents();
            _dialogParentBg.enabled = false;
        }

        public void ShowDialog(
            RectTransform prefab,
            Vector2? startPos = null)
        {
            var dialog = prefab.Instantiate(_dialogParentBg.RT());

            if (_stack.Count == 0)
                _view
                    .GetComponentsInChildren<InputDetector>()
                    .ForEach(i => i.Interactable = false);

            _stack.Push(new()
            {
                InitialPos = startPos.HasValue ? startPos.Value : ScreenEx.Center,
                Instance = dialog
            });

            dialog.position = startPos.HasValue ? startPos.Value : ScreenEx.Center;
            dialog.localScale = Vector3.zero;
            UILayoutRebuilder.Rebuild(dialog.gameObject);

            LerpDialog(dialog, ScreenEx.Center, 1, onDone: () =>
            {
                _dialogParentBg.enabled = true;
            });
        }

        public void Back()
        {
            var dialogInfo = _stack.Pop();

            LerpDialog(dialogInfo.Instance, dialogInfo.InitialPos, 0, onDone: () =>
            {
                dialogInfo.Instance.Destroy();
                _dialogParentBg.enabled = false;

                if (_stack.Count == 0)
                    _view
                        .GetComponentsInChildren<InputDetector>()
                        .ForEach(i => i.Interactable = true);
            });
        }

        private void LerpDialog(
            RectTransform rt, 
            Vector2 targetPos, 
            float targetScale,
            float time = 0.5f,
            Action onDone = null)
        {
            LerpFunctions.BeginLerp(rt, restore =>
            {
                LerpFunctions.LerpPosition2D(
                   _startCoroutine,
                   rt,
                   targetPos,
                   time,
                   onDone: onDone);

                LerpFunctions.LerpScale2D(
                   _startCoroutine,
                   rt,
                   targetScale,
                   time);
            });
        }

        public bool Any() => _stack.Count > 0;

        private class DialogInfo
        {
            public Vector2 InitialPos { get; init; }
            public RectTransform Instance { get; init; }

        }
    }
}
