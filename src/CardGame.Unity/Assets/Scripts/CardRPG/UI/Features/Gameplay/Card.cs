using Core.Collections;
using Core.Functional;
using Core.Unity.Transforms;
using Core.Unity.UI;
using Core.Unity.Math;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField] private CardRpgIOs.CardIOList _reversedCardsIO;

        private Entities.Gameplay.Card _card;
        private bool _isEnemy;

        public RectTransform RT { get; private set; }
        private RectTransform _moveArea;

        private static CardImages _cardImages;

        private void Awake()
        {
            RT = GetComponent<RectTransform>();
            _cardImages ??= FindObjectOfType<CardImages>();
            _moveArea = GameObject.FindGameObjectWithTag("MoveArea").RT();
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
                if (_isMoving)
                    return;
                
                OnCardSelected?.Invoke(_card, _isEnemy);
            });

            _reversedCardButton.onClick.RemoveAllListeners();
            _reversedCardButton.onClick.AddListener(() => 
            {
                if (_isMoving)
                    return;
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

        private bool _isMoving;
        public void AnimateMixingCards(bool isMe)
        {
            var initialPos = RT.position;

            var targetPos = new Vector2(
                Screen.width / 2 + (RT.rect.width + 10) * (isMe ? 1 : -1),
                Screen.height / 2);

            LerpFunctions.BeginLerp(RT, _moveArea, onDone =>
            {
                LerpFunctions.LerpScale2D(
                    StartCoroutine,
                    RT,
                    1.3f);

                LerpFunctions.LerpPosition2D(
                    StartCoroutine,
                    RT,
                    targetPos,
                    onDone: AnimateCascade);
            });

            void AnimateCascade()
            {
                var cardMoveTime = 0.25f;
                var timeOffsetFactor = 0.66f;
                var cardCount = 7;
                var totalTime = cardCount * cardMoveTime * timeOffsetFactor;

                if (isMe)
                    AnimateDeckCascade(ScreenEx.Middle, cardMoveTime, timeOffsetFactor, cardCount);
                else
                    StartCoroutine(CRT());

                StartCoroutine(MoveOwnDecksBack(totalTime));

                IEnumerator CRT()
                {
                    yield return new WaitForSeconds(cardMoveTime / 2);
                    AnimateDeckCascade(ScreenEx.Middle, cardMoveTime, timeOffsetFactor, cardCount);
                }
            }

            IEnumerator MoveOwnDecksBack(float waitTime)
            {
                yield return new WaitForSeconds(waitTime);

                LerpFunctions.BeginLerp(RT, _moveArea, onDone =>
                {
                    LerpFunctions.LerpScale2D(
                        StartCoroutine,
                        RT,
                        1f);

                    LerpFunctions.LerpPosition2D(
                        StartCoroutine,
                        RT,
                        initialPos,
                        onDone: () => 
                        {
                            isMe.IfFalseDo(() => _reversedCardsIO.Destroy());
                            isMe.IfTrueDo(MoveCommonDeckBack);
                        });
                });
            }

            void MoveCommonDeckBack()
            {
                _reversedCardsIO.Destroy(1, int.MaxValue);
                var card = _reversedCardsIO.Object;

                LerpFunctions.BeginLerp(card.RT, _moveArea, onDone =>
                {
                    LerpFunctions.LerpPosition2D(
                        StartCoroutine,
                        card.RT,
                        ScreenEx.Right.AddX(-card.RT.rect.width / 2 - 10));

                LerpFunctions.LerpScale2D(
                        StartCoroutine,
                        card.RT,
                        1f);
                });
            }
        }

        public void AnimateDeckCascade(
            Vector2 targetPos, float cardMoveTime, float timeOffsetFactor = 1f, int count = 5)
        {
            count.ForEach(i =>
            {
                StartCoroutine(CRT(cardMoveTime, i * cardMoveTime * timeOffsetFactor));
            });

            IEnumerator CRT(float cardMoveTime, float waitTime)
            {
                yield return new WaitForSeconds(waitTime);

                var card = _reversedCardsIO.Instantiate();
                card.RT.localScale = new Vector3(1, 1, 1);
                card.RT.pivot = new Vector2(0.5f, 0.5f);
                RTPresetsExtensions.StretchToMiddle(card);

                LerpFunctions.BeginLerp(card.RT, _moveArea, onDone =>
                    LerpFunctions.LerpPosition2D(
                        StartCoroutine,
                        card.RT,
                    targetPos,
                    durationSeconds: cardMoveTime));
            }
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
