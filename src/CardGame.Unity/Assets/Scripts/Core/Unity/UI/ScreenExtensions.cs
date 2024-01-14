using UnityEngine;

namespace Core.Unity.UI
{
    public static class ScreenEx
    {
        public static Vector2 Size = new Vector2(Screen.width, Screen.height);
        public static Vector2 Middle = new Vector2(Screen.width / 2, Screen.height / 2);
        public static Vector2 Right = new Vector2(Screen.width, Screen.height / 2);
    }
}
