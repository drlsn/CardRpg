using Core.Unity.Transforms;
using Core.Unity.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Core.Maths.LerpingFunctions;

namespace CardRPG.UI.Gameplay
{
    public class Card : MonoBehaviour
    {
        public delegate void OnCardSelectedDelegate(Entities.Gameplay.Card card, bool isEnemy);
        public event OnCardSelectedDelegate OnCardSelected;

        [SerializeField] private TMP_Text _nameText;

        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private TMP_Text _attackText;

        [SerializeField] private Button _cardButton;
        [SerializeField] private Button _reversedCardButton;

        [SerializeField] private Image _image;
        
        private Entities.Gameplay.Card _card;
        private bool _isEnemy;

        private RectTransform _rt;
        private GameObject _moveArea;

        private static CardImages _cardImages;

        private void Awake()
        {
            _rt = GetComponent<RectTransform>();
            _cardImages ??= FindObjectOfType<CardImages>();
            _moveArea = GameObject.FindGameObjectWithTag("MoveArea");

            _cardButton.onClick.RemoveAllListeners();
            _cardButton.onClick.AddListener(() =>
            {
                OnCardSelected?.Invoke(_card, _isEnemy);
                if (_cardForMove is null || _isMoving || _cardForMove == this)
                    return;

                StartMoving(_rt.position);
            });

            _reversedCardButton?.onClick?.RemoveAllListeners();
            _reversedCardButton?.onClick?.AddListener(() =>
            {
                if (_cardForMove is null || _isMoving || _cardForMove == this)
                    return;

                StartMoving(_rt.position);
            });
        }

        public void Init(Entities.Gameplay.Card card, bool isEnemy)
        {
            _card = card;
            _isEnemy = isEnemy;

            _nameText.text = card.Name;
            _hpText.text = card.Statistics.HP.CalculatedValue.ToString();// + " HP";
            _attackText.text = card.Statistics.Attack.CalculatedValue.ToString();// + " AT";

            _image.sprite = _cardImages.Sprites[card.ImageIndex];

            _cardButton.onClick.RemoveAllListeners();
            _cardButton.onClick.AddListener(() =>
            {
                OnCardSelected?.Invoke(_card, _isEnemy);
                if (_cardForMove is null || _isMoving)
                    return;

                Vector3[] v = new Vector3[4];
                _rt.GetWorldCorners(v);
                StartMoving(v[0] + new Vector3(0, (v[1].y - v[0].y) / 2));
            });

            _reversedCardButton.onClick.RemoveAllListeners();
            _reversedCardButton.onClick.AddListener(() => 
            {
                if (_cardForMove is null || _isMoving)
                    return;

                Vector3[] v = new Vector3[4];
                _rt.GetWorldCorners(v);
                StartMoving(v[0] + new Vector3(0, (v[1].y - v[0].y) / 2));
            });
        }

        public void Refresh(Entities.Gameplay.Card card, bool isEnemy)
        {
            _card = card;
            _isEnemy = isEnemy;

            _nameText.text = card.Name;
            _hpText.text = card.Statistics.HP.CalculatedValue.ToString();// + " HP";
            _attackText.text = card.Statistics.Attack.CalculatedValue.ToString();// + " AT";

            Debug.Log($"{card.Name} - Image index - {card.ImageIndex}");
            _image.sprite = _cardImages.Sprites[card.ImageIndex];
            _image.rectTransform.sizeDelta = new(1000, _image.rectTransform.rect.height);

            _cardButton.onClick.AddListener(() => OnCardSelected?.Invoke(_card, _isEnemy));
        }

        private static Card _cardForMove;

        public void SelectForMove()
        {
            _cardForMove = this;
        }

        public void SetBgButtonActive(bool enabled = true)
        {
            _reversedCardButton.transform.parent.gameObject.SetActive(enabled);
        }

        private bool _isMoving;
        public void StartMoving(Vector2 targetPos)
        {
            if (_isMoving)
                return;
            Debug.Log("StartMoving");

            var rt = _cardForMove.transform.GetComponent<RectTransform>();
            var prevParent = rt.parent;
            rt.SetParent(_moveArea.transform);
            _isMoving = true;

            var time = 0.75f;
            LerpFunctions.LerpPosition2D(
                rt,
                targetPos,
                durationSeconds: time,
                LerpFunctionType.Smooth,
                StartCoroutine,
                onDone: () =>
                {
                    _isMoving = false;
                    rt.SetParent(prevParent);
                    _cardForMove.SetBgButtonActive(false);
                    _cardForMove = null;
                });

            LerpFunctions.LerpRotationZ(
                rt,
                360,
                durationSeconds: time,
                LerpFunctionType.Smooth,
                StartCoroutine,
                onDone: () => { });

            LerpFunctions.LerpScale2D(
                    rt,
                    2,
                    durationSeconds: time / 2,
                    LerpFunctionType.Smooth,
                    StartCoroutine,
                    onDone: () => 
                    {
                        LerpFunctions.LerpScale2D(
                        rt,
                        1,
                        durationSeconds: time / 2,
                        LerpFunctionType.Smooth,
                        StartCoroutine,
                        onDone: () => { });
                    });
        }
    }
}

public enum CardMoveTarget
{
    EnemyHand
}

public static class RandomExtensions
{
    public static T GetRandom<T>(this IList<T> source, System.Random random = null)
    {
        random ??= new System.Random();
        int idx = random.Next(source.Count() - 1);
        return source[idx];
    }
}
