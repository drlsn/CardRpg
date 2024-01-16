using UnityEngine.UI;

namespace Core.Unity.UI
{
    public static class ButtonExtensions
    {
        public static Button RemoveListeners(this Button button)
        {
            button.onClick.RemoveAllListeners();
            return button;
        }

        public static Button DisableAndRemoveListeners(this Button button)
        {
            button.RemoveListeners();
            button.enabled = false;
            return button;
        }
    }
}
