using System;
using TMPro;
using UnityEngine;

namespace Core.Unity
{
    public static class UnityIOs
    {
        [Serializable] public class TransformIO : InstantiationObject<Transform>, IInstantiationObject<Transform> {}
        [Serializable] public class TransformIOList : InstantiationObjectList<Transform>, IInstantiationObjectList<Transform> {}
        [Serializable] public class InputFieldTmpIO : InstantiationObject<TMP_InputField>, IInstantiationObject<TMP_InputField> { }
        [Serializable] public class TextTmpIO : InstantiationObject<TMP_Text>, IInstantiationObject<TMP_Text> { }
        [Serializable] public class TextTmpIOList : InstantiationObjectList<TMP_Text>, IInstantiationObjectList<TMP_Text> { }
    }
}
