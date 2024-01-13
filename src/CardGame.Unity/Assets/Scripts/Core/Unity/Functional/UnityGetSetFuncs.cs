using Core.Unity.Transforms;
using Core.Unity.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Unity.Functional
{
    public static class UnityGetSetFuncs
    {
        public static Action<float> SetRTWidthAction(RectTransform component) =>
                value => component.SetWidth(value);

        public static Func<float> GetRTWidthFunc(RectTransform component) =>
            () => component.sizeDelta.x;

        public static Action<float> SetRTHeightAction(RectTransform component) =>
                value => component.SetHeight(value);

        public static Func<float> GetRTHeightFunc(RectTransform component) =>
            () => component.sizeDelta.y;

        public static Action<float> SetTransformRotationZAction(Transform component) =>
                value => component.SetRotationZ(value);

        public static Func<float> GetTransformRotationZFunc(Transform component) =>
         () => component.rotation.eulerAngles.z;

        public static Func<Vector2> GetTransformScale2DFunc(Transform component) =>
            () => component.localScale;

        public static Action<Vector2> SetTransformScale2DAction(Transform component) =>
                value => component.SetLocalScale(value);

        public static Action<Vector2> SetTransformPosition2DAction(Transform component) =>
                value => component.SetPosition(value);

        public static Func<Vector2> GetTransformPosition2DFunc(Transform component) =>
            () => component.position;

        public static Action<Vector2> SetAnchoredPosition2DAction(RectTransform component) =>
                value => component.anchoredPosition = value;

        public static Func<Vector2> GetAnchoredPosition2DFunc(RectTransform component) =>
            () => component.anchoredPosition;

        public static Action<Vector2> SetTransformLocalPosition2DAction(Transform component) =>
                value => component.SetLocalPosition(value);

        public static Func<Vector2> GetTransformLocalPosition2DFunc(Transform component) =>
            () => component.localPosition;

        public static Action<Color> SetGraphicColorAction(Graphic component) =>
            value => component.color = value;

        public static Func<Color> GetGraphicColorFunc(Graphic component) =>
            () => component.color;

        public static Action<float> SetImageFillAction(Image component) =>
            value => component.fillAmount = value;

        public static Func<float> GetImageFillFunc(Image component) =>
            () => component.fillAmount;

        public static Action<float> SetCameraSizeAction(UnityEngine.Camera component) =>
            value => component.orthographicSize = value;

        public static Func<float> GetCameraSizeFunc(UnityEngine.Camera component) =>
            () => component.orthographicSize;

        public static Action<float> SetAnimatorSpeedAction(Animator component) =>
            value => component.speed = value;

        public static Func<float> GetAnimatorSpeedFunc(Animator component) =>
            () => component.speed;

        public static Action<float> SetCurrentAnimationTimeAction(Animator component) 
        {
            int animHash = component.GetCurrentAnimatorStateInfo(0).tagHash;
            return value => component.Play(animHash, 0, value);
        }

        public static Func<float> GetCurrentAnimationTimeFunc(Animator component) =>
            () =>
            {
                float normalizedTime = component.GetCurrentAnimatorStateInfo(0).normalizedTime;
                float loopProgress = normalizedTime % 1;

                return loopProgress;
            };
    }
}
