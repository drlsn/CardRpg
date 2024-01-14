using Common.Unity.Functional;
using Core.Collections;
using Core.Maths;
using Core.Unity.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Core.Maths.LerpingFunctions;

namespace Core.Unity.Transforms
{
    public class LerpFunctions
    {
        #region Lerp Specific

        public static void LerpAnimatorSpeed(
           Animator animator,
           float targetValue,
           float durationSeconds,
           LerpFunctionType type,
           Func<IEnumerator, Coroutine> startCoroutine,
           Action onDone)
        {
            LerpingFunctions.Lerp(
                Mathf.Lerp,
                UnityGetSetFuncs.GetAnimatorSpeedFunc(animator),
                UnityGetSetFuncs.SetAnimatorSpeedAction(animator),
                targetValue,
                durationSeconds,
                startCoroutine,
                UnityGlobalStateFuncs.GetDeltaTime,
                LerpingFunctions.GetLerpFunction(type),
                onDone);
        }

        public static void LerpNestedAnimatorSpeed(
           GameObject animator,
           float targetValue,
           float durationSeconds,
           LerpFunctionType type,
           Func<IEnumerator, Coroutine> startCoroutine,
           Action onDone)
        {
            var nestedComponents =
                animator.GetThisAndNestedChildren<Animator>();

            foreach (var child in nestedComponents)
                LerpingFunctions.Lerp(
                    Mathf.Lerp,
                    UnityGetSetFuncs.GetAnimatorSpeedFunc(child),
                    UnityGetSetFuncs.SetAnimatorSpeedAction(child),
                    targetValue,
                    durationSeconds,
                    startCoroutine,
                    UnityGlobalStateFuncs.GetDeltaTime,
                    LerpingFunctions.GetLerpFunction(type),
                    onDone);
        }

        public static void LerpNestedAnimatorTime(
           GameObject animator,
           float targetValue,
           float durationSeconds,
           LerpFunctionType type,
           Func<IEnumerator, Coroutine> startCoroutine,
           Action onDone)
        {
            var nestedComponents =
                animator.GetThisAndNestedChildren<Animator>();

            nestedComponents.ForEach((a, i) => a.speed = 0);

            foreach (var child in nestedComponents)
                LerpingFunctions.Lerp(
                    Mathf.Lerp,
                    UnityGetSetFuncs.GetCurrentAnimationTimeFunc(child),
                    UnityGetSetFuncs.SetCurrentAnimationTimeAction(child),
                    targetValue,
                    durationSeconds,
                    startCoroutine,
                    UnityGlobalStateFuncs.GetDeltaTime,
                    LerpingFunctions.GetLerpFunction(type),
                    onDone);
        }

        public static void LerpCameraSize(
            Camera camera,
            float targetValue,
            float durationSeconds,
            LerpFunctionType type,
            Func<IEnumerator, Coroutine> startCoroutine,
            Action onDone)
        {
            LerpingFunctions.Lerp(
                Mathf.Lerp,
                UnityGetSetFuncs.GetCameraSizeFunc(camera),
                UnityGetSetFuncs.SetCameraSizeAction(camera),
                targetValue,
                durationSeconds,
                startCoroutine,
                UnityGlobalStateFuncs.GetDeltaTime,
                LerpingFunctions.GetLerpFunction(type),
                onDone);
        }

        public static void LerpImageFill(
            Image image,
            float targetValue,
            float durationSeconds,
            LerpFunctionType type,
            Func<IEnumerator, Coroutine> startCoroutine,
            Action onDone)
        {
            LerpingFunctions.Lerp(
                Mathf.Lerp,
                UnityGetSetFuncs.GetImageFillFunc(image),
                UnityGetSetFuncs.SetImageFillAction(image),
                targetValue,
                durationSeconds,
                startCoroutine,
                UnityGlobalStateFuncs.GetDeltaTime,
                LerpingFunctions.GetLerpFunction(type),
                onDone);
        }

        public static Coroutine LerpRTWidth(
            RectTransform rt,
            float targetValue,
            float durationSeconds,
            LerpFunctionType type,
            Func<IEnumerator, Coroutine> startCoroutine,
            Action onDone)
        {
            return LerpingFunctions.Lerp(
                Mathf.Lerp,
                UnityGetSetFuncs.GetRTWidthFunc(rt),
                UnityGetSetFuncs.SetRTWidthAction(rt),
                targetValue,
                durationSeconds,
                startCoroutine,
                UnityGlobalStateFuncs.GetDeltaTime,
                LerpingFunctions.GetLerpFunction(type),
                onDone);
        }

        public static Coroutine LerpRTHeight(
            RectTransform rt,
            float targetValue,
            float durationSeconds,
            LerpFunctionType type,
            Func<IEnumerator, Coroutine> startCoroutine,
            Action onDone = null)
        {
            return LerpingFunctions.Lerp(
                Mathf.Lerp,
                UnityGetSetFuncs.GetRTHeightFunc(rt),
                UnityGetSetFuncs.SetRTHeightAction(rt),
                targetValue,
                durationSeconds,
                startCoroutine,
                UnityGlobalStateFuncs.GetDeltaTime,
                LerpingFunctions.GetLerpFunction(type),
                onDone);
        }

        public static void LerpPosition2D(
            Transform transform,
            Vector2 targetPosition,
            float durationSeconds,
            LerpFunctionType type,
            Func<IEnumerator, Coroutine> startCoroutine,
            Action onDone)
        {
            LerpingFunctions.Lerp(
                Vector2.Lerp,
                UnityGetSetFuncs.GetTransformPosition2DFunc(transform),
                UnityGetSetFuncs.SetTransformPosition2DAction(transform),
                targetPosition,
                durationSeconds,
                startCoroutine,
                UnityGlobalStateFuncs.GetDeltaTime,
                LerpingFunctions.GetLerpFunction(type),
                onDone);
        }

        public static void LerpRotationZ(
            Transform transform,
            float targetValue,
            float durationSeconds,
            LerpFunctionType type,
            Func<IEnumerator, Coroutine> startCoroutine,
            Action onDone)
        {
            LerpingFunctions.Lerp(
                Mathf.Lerp,
                UnityGetSetFuncs.GetTransformRotationZFunc(transform),
                UnityGetSetFuncs.SetTransformRotationZAction(transform),
                targetValue,
                durationSeconds,
                startCoroutine,
                UnityGlobalStateFuncs.GetDeltaTime,
                LerpingFunctions.GetLerpFunction(type),
                onDone);
        }

        public static void LerpLocalPosition2D(
            Transform transform,
            Vector2 targetPosition,
            float durationSeconds,
            LerpFunctionType type,
            Func<IEnumerator, Coroutine> startCoroutine,
            Action onDone)
        {
            LerpingFunctions.Lerp(
                Vector2.Lerp,
                UnityGetSetFuncs.GetTransformLocalPosition2DFunc(transform),
                UnityGetSetFuncs.SetTransformLocalPosition2DAction(transform),
                targetPosition,
                durationSeconds,
                startCoroutine,
                UnityGlobalStateFuncs.GetDeltaTime,
                LerpingFunctions.GetLerpFunction(type),
                onDone);
        }

        public static void LerpAnchoredPosition2D(
            RectTransform rt,
            Vector2 targetPosition,
            float durationSeconds,
            LerpFunctionType type,
            Func<IEnumerator, Coroutine> startCoroutine,
            Action onDone = null)
        {
            LerpingFunctions.Lerp(
                Vector2.Lerp,
                UnityGetSetFuncs.GetAnchoredPosition2DFunc(rt),
                UnityGetSetFuncs.SetAnchoredPosition2DAction(rt),
                targetPosition,
                durationSeconds,
                startCoroutine,
                UnityGlobalStateFuncs.GetDeltaTime,
                LerpingFunctions.GetLerpFunction(type),
                onDone);
        }

        public static void LerpScale2D(
            Transform transform,
            float targetValue,
            float durationSeconds,
            LerpFunctionType type,
            Func<IEnumerator, Coroutine> startCoroutine,
            Action onDone)
        {
            LerpingFunctions.Lerp(
                Vector2.Lerp,
                UnityGetSetFuncs.GetTransformScale2DFunc(transform),
                UnityGetSetFuncs.SetTransformScale2DAction(transform),
                new Vector2(targetValue, targetValue),
                durationSeconds,
                startCoroutine,
                UnityGlobalStateFuncs.GetDeltaTime,
                LerpingFunctions.GetLerpFunction(type),
                onDone);
        }

        public static void LerpColor(
            Graphic image,
            Color targetColor,
            float durationSeconds,
            LerpFunctionType type,
            Func<IEnumerator, Coroutine> startCoroutine,
            Action onDone)
        {
            LerpingFunctions.Lerp(
                Color.Lerp,
                UnityGetSetFuncs.GetGraphicColorFunc(image),
                UnityGetSetFuncs.SetGraphicColorAction(image),
                targetColor,
                durationSeconds,
                startCoroutine,
                UnityGlobalStateFuncs.GetDeltaTime,
                LerpingFunctions.GetLerpFunction(type),
                onDone);
        }

        public static void LerpVisibleAndChildrenM(
            IEnumerable<GameObject> goWithVisibleOrChildrenEnumerable,
            Color targetColor,
            float durationSeconds,
            LerpFunctionType type,
            Func<IEnumerator, Coroutine> startCoroutine,
            Action onDone)
        {
            goWithVisibleOrChildrenEnumerable
                .ForEach(go => LerpVisibleAndChildrenM(go, targetColor, durationSeconds, type, startCoroutine, onDone));
        }

        public static void LerpVisibleAndChildrenM(
            GameObject goWithVisibleOrChildren,
            Color targetColor,
            float durationSeconds,
            LerpFunctionType type,
            Func<IEnumerator, Coroutine> startCoroutine,
            Action onDone)
        {
            var visibleComponents =
                goWithVisibleOrChildren.GetThisAndNestedChildren<Graphic>();

            foreach (var visible in visibleComponents)
                LerpFunctions.LerpColor(
                    visible, targetColor, durationSeconds, type,
                    startCoroutine, onDone);
        }

        #endregion

        #region Lerp Complex

        public static void LerpPosition2DComplex(
            RectTransform rt,
            RectTransform moveParent,
            Vector2 targetPosition,
            Func<IEnumerator, Coroutine> startCoroutine,
            Action onDone = null,
            float durationSeconds = 0.75f,
            LerpFunctionType type = LerpFunctionType.Smooth)
        {
            var previousParent = rt.parent;
            rt.SetParent(moveParent.transform);

            LerpPosition2D(
                rt,
                targetPosition,
                durationSeconds,
                type,
                startCoroutine,
                onDone: () => 
                {
                    rt.SetParent(previousParent);
                    onDone?.Invoke();
                });
        }

        #endregion

        public static void ToBlack(Image image)
        {
            image.color = Color.black;
        }
    }
}
