using UnityEngine;

namespace Core.Unity.UI
{
    public class FlexListItem : MonoBehaviour
    {
        [SerializeField] private bool _ignoreLayout;

        public bool IgnoreLayout => _ignoreLayout;
    }
}