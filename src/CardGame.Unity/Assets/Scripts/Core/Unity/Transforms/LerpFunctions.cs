using Common.Unity.Functional;
using Core.Collections;
using Core.Maths;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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

        public static Coroutine LerpPosition2D(
            Func<IEnumerator, Coroutine> startCoroutine,
            Transform transform,
            Vector2 targetPosition,
            float durationSeconds = 0.75f,
            LerpFunctionType type = LerpFunctionType.Smooth,
            Action onDone = null,
            Action<float> onFrame = null)
        {
            return LerpingFunctions.Lerp(
                Vector2.Lerp,
                UnityGetSetFuncs.GetTransformPosition2DFunc(transform),
                UnityGetSetFuncs.SetTransformPosition2DAction(transform),
                targetPosition,
                durationSeconds,
                startCoroutine,
                UnityGlobalStateFuncs.GetDeltaTime,
                LerpingFunctions.GetLerpFunction(type),
                onDone,
                onFrame);
        }

        public static Coroutine LerpRotationZ(
            Func<IEnumerator, Coroutine> startCoroutine,
            Transform transform,
            float targetValue,
            float durationSeconds = 0.75f,
            LerpFunctionType type = LerpFunctionType.Smooth,
            Action onDone = null)
        {
            return LerpingFunctions.Lerp(
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
            Func<IEnumerator, Coroutine> startCoroutine,
            RectTransform rt,
            Vector2 targetPosition,
            float durationSeconds = 0.75f,
            LerpFunctionType type = LerpFunctionType.Smooth,
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

        public static Coroutine LerpScale2D(
            Func<IEnumerator, Coroutine> startCoroutine,
            Transform transform,
            float targetValue,
            float durationSeconds = 0.75f,
            LerpFunctionType type = LerpFunctionType.Smooth,
            Action onDone = null)
        {
            return LerpingFunctions.Lerp(
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

        public static Coroutine LerpScaleX(
            Func<IEnumerator, Coroutine> startCoroutine,
            Transform transform,
            float targetValue,
            float durationSeconds = 0.75f,
            LerpFunctionType type = LerpFunctionType.Smooth,
            Action onDone = null)
        {
            return LerpingFunctions.Lerp(
                Mathf.Lerp,
                UnityGetSetFuncs.GetTransformScaleXFunc(transform),
                UnityGetSetFuncs.SetTransformScaleXAction(transform),
                targetValue,
                durationSeconds,
                startCoroutine,
                UnityGlobalStateFuncs.GetDeltaTime,
                LerpingFunctions.GetLerpFunction(type),
                onDone);
        }

        public static Coroutine LerpScaleY(
            Func<IEnumerator, Coroutine> startCoroutine,
            Transform transform,
            float targetValue,
            float durationSeconds = 0.75f,
            LerpFunctionType type = LerpFunctionType.Smooth,
            Action onDone = null)
        {
            return LerpingFunctions.Lerp(
                Mathf.Lerp,
                UnityGetSetFuncs.GetTransformScaleYFunc(transform),
                UnityGetSetFuncs.SetTransformScaleYAction(transform),
                targetValue,
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

        public static void BeginLerp(
            RectTransform rt,
            RectTransform moveParent, 
            Action<Action> lerpAction)
        {
            var previousPivot = rt.pivot;
            var previousParent = rt.parent;

            if (rt.parent != moveParent)
            {
                var previousPos = rt.position;

                rt.pivot = new Vector2(0.5f, 0.5f);
                var offset = (rt.pivot - previousPivot) * new Vector2(rt.rect.width * rt.lossyScale.x, rt.rect.height * rt.lossyScale.x);

                rt.SetParent(moveParent.transform, worldPositionStays: true);

                var newPos = previousPos;
                newPos.x += offset.x;
                newPos.y += offset.y;
                rt.position = newPos;
            }

            lerpAction(() =>
            {
                if (rt.parent != moveParent)
                {
                    rt.SetParent(previousParent, worldPositionStays: false);
                    rt.pivot = previousPivot;
                }
            });
        }

        #endregion

        public static void ToBlack(Image image)
        {
            image.color = Color.black;
        }
    }
}
