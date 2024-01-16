using CardRPG.UI.Features.Gameplay;
using Common.Unity.Coroutines;
using Core.Basic;
using Core.Collections;
using Core.Functional;
using Core.Unity;
using Core.Unity.Coroutines;
using Core.Unity.Transforms;
using Core.Unity.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        public RectTransform RT;

        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descText;

        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private TMP_Text _attackText;

        [SerializeField] private Button _cardButton;
        public Button ReversedCardButton;

        [SerializeField] private Image _image;

        [SerializeField] private CardRpgIOs.CardIOList _reversedCardsIO;

        [SerializeField] private ArrowTransitionController _arrowController;

        private Entities.Gameplay.Card _card;
        private bool _isEnemy;
        private RectTransform _moveArea;

        private static CardImages _cardImages;

        public static Card[] operator +(Card left, Card right) =>
            new[] { left, right };

        private void Awake()
        {
            _cardImages ??= FindObjectOfType<CardImages>();
            _moveArea = GameObject.FindGameObjectWithTag("MoveArea").RT();
            _cm = new(StopCoroutine);
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
                if (!_moveOrderCounter.IsEmpty)
                    return;
                
                OnCardSelected?.Invoke(_card, _isEnemy);
            });

            ReversedCardButton.onClick.RemoveAllListeners();
            ReversedCardButton.onClick.AddListener(() => 
            {
                if (!_moveOrderCounter.IsEmpty)
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

        public void SetDesc(string text) => _descText.text = text;

        public Card ShowArrow() => _arrowController.Show().ThenReturn(this);
        public Card HideArrow() => _arrowController.Hide().ThenReturn(this);

        public void GrayOn() 
        {
            var grayGO = new GameObject("Gray");
            grayGO
                .AddComponent<Image>()
                .color = new Color(0, 0, 0, 0.5f);

            grayGO.transform.parent = transform;
            grayGO.transform.StretchToExtents();
            grayGO.transform.localScale = Vector3.one;
            grayGO.transform.AddExtents(-2);
        }

        public void GrayOff()
        {
            this.Remove<Image>();
        }

        private Counter _moveOrderCounter = new();
        private OrderedDictionary _moveOrderTimes = new();

        public void AnimateMixingCards(
            bool isMe,
            RectTransform commonDeckTarget,
            float cardMoveTime = 0.75f,
            float mixCardMoveTime = 0.25f,
            float cascadeCardTimeOffsetFactor = 0.66f,
            int cardCount = 7,
            Action onDone = null)
        {
            var initialPos = RT.position;

            var targetPos = _moveArea.GetScreenPos(xOffset: RT.rect.width * 2 * (isMe ? -1 : 1));
             
            LerpFunctions.BeginLerp(RT, _moveArea, restore =>
            {
                LerpFunctions.LerpPosition2D(   
                    StartCoroutine,
                    RT,
                    targetPos,
                    cardMoveTime,
                    onDone: () => AnimateCascade(MoveOwnDeckBack));

                initialPos = RT.position;
                LerpFunctions.LerpRotationZ(
                    StartCoroutine,
                    RT,
                    360,
                    cardMoveTime);

                LerpFunctions.LerpScale2D(
                    StartCoroutine,
                    RT,
                    1.5f,
                    cardMoveTime / 2,
                    onDone: () =>
                        LerpFunctions.LerpScale2D(
                            StartCoroutine,
                            RT,
                            1f,
                            cardMoveTime / 2));
            });

            void AnimateCascade(Action onDone)
            {
                if (isMe)
                    AnimateDeckCascade(ScreenEx.Middle, mixCardMoveTime, cascadeCardTimeOffsetFactor, cardCount, onDone);
                else
                    CoroutineExtensions.RunAsCoroutine(
                        () => AnimateDeckCascade(ScreenEx.Middle, mixCardMoveTime, cascadeCardTimeOffsetFactor, cardCount, onDone.Then(MoveCommonDeckBack)),  
                        delaySeconds: mixCardMoveTime / 2,
                        StartCoroutine);
            }

            void MoveOwnDeckBack()
            {
                isMe.IfTrueDo(_reversedCardsIO.Destroy);
                LerpFunctions.BeginLerp(RT, _moveArea, restore =>
                {
                    LerpFunctions.LerpPosition2D(
                        StartCoroutine,
                        RT,
                        initialPos,
                        cardMoveTime);

                    LerpFunctions.LerpRotationZ(
                        StartCoroutine,
                        RT,
                        -360,
                        cardMoveTime);

                    LerpFunctions.LerpScale2D(
                        StartCoroutine,
                        RT,
                        1.5f,
                        cardMoveTime / 2,
                        onDone: () =>
                            LerpFunctions.LerpScale2D(
                                StartCoroutine,
                                RT,
                                1f,
                                cardMoveTime / 2));
                });
            }

            void MoveCommonDeckBack()
            {
                _reversedCardsIO.Destroy(1, int.MaxValue);
                var card = _reversedCardsIO.Object;
                card.SetDesc("Common\nDeck");

                LerpFunctions.BeginLerp(card.RT, _moveArea, restore =>
                {
                    LerpFunctions.LerpPosition2D(
                        StartCoroutine,
                        card.RT,
                        commonDeckTarget.GetScreenPos(),
                        cardMoveTime,
                        onDone: onDone);
                });
            }
        }

        public void AnimateDeckCascade(
            Vector2 targetPos, float cascadeTime, float timeOffsetFactor = 1f, int count = 5, Action onDone = null)
        {
            var cardMoveTime = cascadeTime / 2;
            count.ForEach(i =>
            {
                StartCoroutine(
                    CRT(cardMoveTime, i * cardMoveTime * timeOffsetFactor, 
                        onDone: i == count - 1 ? onDone : null));
            });

            IEnumerator CRT(float cardMoveTime, float waitTime, Action onDone)
            {
                yield return new WaitForSeconds(waitTime);

                var card = _reversedCardsIO.Instantiate();
                card.RT.localScale = new Vector3(1, 1, 1);
                RTPresetsExtensions.StretchToMiddle(card);

                LerpFunctions.BeginLerp(card.RT, _moveArea, restore =>
                {
                    LerpFunctions.LerpPosition2D(
                        StartCoroutine,
                        card.RT,
                        targetPos,
                        durationSeconds: cardMoveTime,
                        onDone: onDone);
                });
            }
        }

        private CoroutineAggregate _cm;
        public void TranslateTo(
            Vector2 targetPos,
            float cardMoveTime = 0.75f)
        {
            _cm.Stop();

            var delaySeconds = 0f;
            if (_moveOrderTimes.Count > 0)
                delaySeconds = (float) ((DateTime.UtcNow.Ticks - (long) _moveOrderTimes[_moveOrderTimes.Count - 1]) / 1_000_000);
            _cm += CoroutineExtensions.RunAsCoroutine(() => 
            {
                _cm += LerpFunctions.LerpPosition2D(
                    StartCoroutine,
                    RT,
                    targetPos,
                    onDone: () => _moveOrderCounter.Decrease());
            }, 
            delaySeconds: delaySeconds + 0.1f, 
            StartCoroutine);
            
            _moveOrderCounter.Increase();
        }


        public void MoveTo(
            Vector2 targetPos,
            float cardMoveTime = 0.75f)
        {
            _moveOrderCounter.Increase();

            var timeId = Guid.NewGuid().ToString();
            _moveOrderTimes.Add(timeId, DateTime.UtcNow.Ticks);

            // Translate
            _cm += LerpFunctions.LerpPosition2D(
                StartCoroutine,
                RT,
                targetPos,
                cardMoveTime,
                onDone: () =>
                {
                    _moveOrderCounter.Decrease();
                    _moveOrderTimes.Remove(timeId);
                });

            // Rotate
            _cm += LerpFunctions.LerpRotationZ(
                StartCoroutine,
                RT,
                360,
                cardMoveTime);

            // Scale X
            _cm += LerpFunctions.LerpScaleX(
                StartCoroutine,
                RT,
                0f,
                cardMoveTime / 2,
                onDone: () =>
                {
                    ReversedCardButton.SetActive(false);
                    _descText.SetActive(false);
                    LerpFunctions.LerpScaleX(
                        StartCoroutine,
                        RT,
                        1f,
                        cardMoveTime / 2)
                    .AddTo(_cm);
                });

            // Scale Y
            _cm += LerpFunctions.LerpScaleY(
                StartCoroutine,
                RT,
                1.5f,
                cardMoveTime / 2,
                onDone: () =>
                    LerpFunctions.LerpScaleY(
                        StartCoroutine,
                        RT,
                        1f,
                        cardMoveTime / 2)
                    .AddTo(_cm));
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
