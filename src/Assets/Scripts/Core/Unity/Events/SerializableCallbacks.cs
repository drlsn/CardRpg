using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Common.Unity.Events
{
    // primitives
    [Serializable] public class OnString : UnityEvent<string> { }
    [Serializable] public class GetBool : SerializableCallback<bool> { }


    // Unity
    [Serializable] public class GetGameObject : SerializableCallback<GameObject> { }
    [Serializable] public class OnPointerEventData : UnityEvent<PointerEventData> { }
    [Serializable] public class SetPointerEventData : UnityEvent<Action<PointerEventData>> { }

}
