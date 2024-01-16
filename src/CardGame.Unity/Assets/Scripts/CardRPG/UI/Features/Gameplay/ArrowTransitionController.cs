using Core.Functional;
using Core.Unity;
using Core.Unity.Transforms;
using Core.Unity.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CardRPG.UI.Features.Gameplay
{
    public class ArrowTransitionController : MonoBehaviour
    {
        private RectTransform _rt;

        private bool _isShown;

        [SerializeField] private Image _image;

        private void Awake()
        {
            _image.enabled = false;
            _rt = transform.RT();
        }

        public ArrowTransitionController Show()
        {
            gameObject.SetActive(true);
            _isShown = true;
            _image.enabled = true;
            MoveUp();

            return this;
        }

        public ArrowTransitionController Hide()
        {
            gameObject.SetActive(false);
            _image.enabled = false;
            _isShown = false;

            return this;
        }

        private void MoveUp()
        {
            _rt ??= transform.RT();
            LerpFunctions.LerpPosition2D(
                StartCoroutine,
                _rt,
                new Vector2(_rt.position.x, _rt.position.y + 50),
                0.5f,
                onDone: () => _isShown.IfTrueDo(MoveDown));
        }

        private void MoveDown()
        {
            _rt ??= transform.RT();
            LerpFunctions.LerpPosition2D(
                StartCoroutine,
                _rt,
                new Vector2(_rt.position.x, _rt.position.y - 50),
                0.5f,
                onDone: () => _isShown.IfTrueDo(MoveUp));
        }
    }
}
