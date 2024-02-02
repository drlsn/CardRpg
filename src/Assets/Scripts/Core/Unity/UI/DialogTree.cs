using Core.Collections;
using Core.Functional;
using Core.Unity.Transforms;
using Core.Unity.UI.Taps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Unity.UI
{
    public class DialogTree
    {
        private readonly Transform _view;
        private readonly Func<IEnumerator, Coroutine> _startCoroutine;

        public Image DialogParentBg { get; }

        private readonly Stack<DialogInfo> _stack = new();

        public DialogTree(
            Transform view,
            Image bgPrefab,
            Func<IEnumerator, Coroutine> startCoroutine) 
        {
            _view = view;
            _startCoroutine = startCoroutine;

            DialogParentBg = bgPrefab.Instantiate(view);
            DialogParentBg.transform.position = ScreenEx.Center;
            DialogParentBg.StretchToExtents();
            DialogParentBg.enabled = false;
            DialogParentBg.GetComponent<Button>().onClick
                .AddListener(() => Back());
        }

        public void ShowDialog<T>(
            Func<RectTransform, T> create,
            Action<RectTransform> destroy,
            Vector2? startPos = null)
            where T : Component
        {
            var dialog = create(DialogParentBg.RT());
            var rt = dialog.GetComponent<RectTransform>();

            if (_stack.Count == 0)
                _view
                    .GetComponentsInChildren<InputDetector>()
                    .ForEach(i => i.Interactable = false);

            _stack.Push(new()
            {
                InitialPos = startPos.HasValue ? startPos.Value : ScreenEx.Center,
                Instance = rt,
                Destroy = destroy
            });

            rt.position = startPos.HasValue ? startPos.Value : ScreenEx.Center;
            rt.localScale = Vector3.zero;
            UILayoutRebuilder.Rebuild(dialog.gameObject);

            LerpDialog(rt, ScreenEx.Center, targetScale: 1, onDone: restore =>
            {
                DialogParentBg.enabled = true;
            });
        }

        public void Back()
        {
            if (!_stack.Any())
                return;

            var dialogInfo = _stack.Pop();

            LerpDialog(dialogInfo.Instance, dialogInfo.InitialPos, targetScale: 0, onDone: restore =>
            {
                dialogInfo.Destroy(dialogInfo.Instance);
                DialogParentBg.enabled = false;
                restore();
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
            float time = 0.3f,
            Action<Action> onDone = null)
        {
            LerpFunctions.BeginLerp(rt, sortingIndex: 20000, restore =>
            {
                LerpFunctions.LerpPosition2D(
                   _startCoroutine,
                   rt,
                   targetPos,
                   time,
                   onDone: () => onDone?.Invoke(restore));

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
            public Action<RectTransform> Destroy { get; init; }
        }
    }
}
