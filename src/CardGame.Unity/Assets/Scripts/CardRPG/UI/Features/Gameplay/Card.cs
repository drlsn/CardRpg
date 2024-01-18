using CardRPG.Entities.Gameplay;
using CardRPG.UI.Features.Gameplay;
using Common.Unity.Coroutines;
using Core.Basic;
using Core.Collections;
using Core.Functional;
using Core.Unity;
using Core.Unity.Coroutines;
using Core.Unity.Scripts;
using Core.Unity.Transforms;
using Core.Unity.UI;
using Core.Unity.UI.Taps;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardRPG.UI.Gameplay
{
    public class Card : UnityScript
    {
        public delegate void OnCardSelectedDelegate(Entities.Gameplay.Card card, bool isEnemy);
        public event OnCardSelectedDelegate OnCardSelected;

        public RectTransform RT;

        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descText;

        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private TMP_Text _attackText;

        public SwipeTapHoldController CardButton;
        public SwipeTapHoldController ReversedCardButton;

        [SerializeField] private Image _image;

        //[SerializeField] private CardRpgIOs.CardIOList _reversedCardsIO;

        [SerializeField] private ArrowTransitionController _arrowController;

        public GameObject DustVFX;

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

            int group = 1;
            _image.sprite = _cardImages.GetAvers(1);
            ReversedCardButton.GetComponent<Image>().sprite = _cardImages.GetRevers(0);
        }

        public void Init(Entities.Gameplay.Card card, bool isEnemy)
        {
            _card = card;
            _isEnemy = isEnemy;

            _nameText.text = card.Name;
            _hpText.text = card.Statistics.HP.CalculatedValue.ToString();// + " HP";
            _attackText.text = card.Statistics.Attack.CalculatedValue.ToString();// + " AT";

            //_image.sprite = _cardImages.Sprites[card.ImageIndex];
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
            gameObject
                .GetComponentsInChildren<Image>()
                .FirstOrDefault(x => x.gameObject.name == "Gray")
                .Destroy();
        }

        public bool IsMoving { get; private set; }
        private Counter _moveOrderCounter = new();
        private OrderedDictionary _moveOrderTimes = new();

        public void AnimateMixingCards(
            bool isMe,
            RectTransform commonDeckTarget,
            float cardMoveTime = 0.75f,
            float mixCardMoveTime = 0.25f,
            Action onDone = null)
        {
            var initialPos = RT.position;

            var targetPos = _moveArea.GetScreenPos(xOffset: RT.rect.width * 2 * (isMe ? -1 : 1));
             
            LerpFunctions.BeginLerp(RT, restore =>
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
                    AnimateDeckCascade(ScreenEx.Center, mixCardMoveTime, isMe: true, onDone);
                else
                    AnimateDeckCascade(ScreenEx.Center, mixCardMoveTime, isMe: false, onDone.Then(MoveCommonDeckBack));
            }

            void MoveOwnDeckBack()
            {
                this.DestroyChildren<Card>();
                LerpFunctions.BeginLerp(RT, restore =>
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
                var card = _commonDeckCard;
                
                LerpFunctions.BeginLerp(card.RT, restore =>
                {
                    LerpFunctions.LerpPosition2D(
                        StartCoroutine,
                        card.RT,
                        commonDeckTarget.GetScreenPos(),
                        cardMoveTime,
                        onDone: restore
                            .Then(onDone)
                            .Then(() => _commonDeckCard.Destroy())
                            .Then(() => _commonDeckCard = null));
                });
            }
        }

        private Card _commonDeckCard;
        public void AnimateDeckCascade(
            Vector2 targetPos, float cardMoveTime, bool isMe, Action onDone = null)
        {
            var previousPos = RT.position;
            LerpFunctions.LerpPosition2D(
                StartCoroutine,
                RT,
                targetPos,
                durationSeconds: cardMoveTime,
                onDone: () =>
                {
                    if (!isMe)
                        _commonDeckCard = RT
                            .Instantiate(RT.parent)
                            .Get<Card>()
                            .Then(card => card.SetDesc("Common\nDeck"));

                    LerpFunctions.LerpPosition2D(
                        StartCoroutine,
                        RT,
                        previousPos,
                        durationSeconds: cardMoveTime,
                        onDone: onDone);
                });
        }

        private CoroutineAggregate _cm;
        public void TranslateTo(
            Vector2 targetPos,
            float cardMoveTime = 0.75f)
        {
            //_cm.Stop();

            var delaySeconds = 0f;
            if (_moveOrderTimes.Count > 0)
                delaySeconds = (float) ((DateTime.UtcNow.Ticks - (long) _moveOrderTimes[_moveOrderTimes.Count - 1]) / 1_000_000);

            //Debug.Log($"delay: {delaySeconds}");
            _cm += CoroutineExtensions.RunAsCoroutine(() => 
            {
                _cm += LerpFunctions.LerpPosition2D(
                    StartCoroutine,
                    RT,
                    targetPos,
                    durationSeconds: 0.3f,
                    onDone: () => _moveOrderCounter.Decrease());

                _cm += LerpFunctions.LerpScale2D(
                        StartCoroutine,
                        RT,
                        1,
                        durationSeconds: 0.3f);
            }, 
            delaySeconds: 0,//delaySeconds + 0.1f,
            StartCoroutine);

            _moveOrderTimes.Clear();
            _moveOrderCounter.Increase();
        }

        public void MoveTo(
            Vector2 targetPos,
            bool dontReverse = false,
            float cardMoveTime = 0.75f,
            bool onlyTranslate = false,
            Action onDone = null)
        {
            IsMoving = true;

            _moveOrderCounter.Increase();

            var timeId = Guid.NewGuid().ToString();
            _moveOrderTimes.Add(timeId, DateTime.UtcNow.Ticks);

            LerpFunctions.BeginLerp(RT, restore =>
            {
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
                        IsMoving = false;
                        onDone?.Invoke();
                        restore();
                    });

                // Rotate
                if (!onlyTranslate)
                    _cm += LerpFunctions.LerpRotationZ(
                        StartCoroutine,
                        RT,
                        360,
                        cardMoveTime);

                if (onlyTranslate)
                {
                    _cm += RunAsCoroutine(
                        () => LerpFunctions.LerpScale2D(
                            StartCoroutine,
                            RT,
                            1.5f,
                            cardMoveTime / 2,
                            onDone: () =>
                            {
                                ReversedCardButton.SetActive(dontReverse);
                                _descText.SetActive(false);
                                LerpFunctions.LerpScale2D(
                                    StartCoroutine,
                                    RT,
                                    1f,
                                    cardMoveTime / 2)
                                .AddTo(_cm);
                            }));
                }
                else
                {
                    // Scale X
                    _cm += RunAsCoroutine(
                        () => LerpFunctions.LerpScaleX(
                            StartCoroutine,
                            RT,
                            0f,
                            cardMoveTime / 3,
                            onDone: () =>
                            {
                                ReversedCardButton.SetActive(dontReverse);
                                _descText.SetActive(false);
                                LerpFunctions.LerpScaleX(
                                    StartCoroutine,
                                    RT,
                                    1f,
                                    cardMoveTime / 3)
                                .AddTo(_cm);
                            }),
                        delaySeconds: cardMoveTime / 3);

                    // Scale Y
                    _cm += RunAsCoroutine(
                        () => LerpFunctions.LerpScaleY(
                            StartCoroutine,
                            RT,
                            1.5f,
                            cardMoveTime / 3,
                            onDone: () =>
                                LerpFunctions.LerpScaleY(
                                    StartCoroutine,
                                    RT,
                                    1f,
                                    cardMoveTime / 3)
                                .AddTo(_cm)),
                            delaySeconds: cardMoveTime / 3);
                }
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

    public static bool GetRandomBool(System.Random random = null)
    {
        random ??= new System.Random();
        return random.Next(2) == 0 ? false : true;
    }
}
