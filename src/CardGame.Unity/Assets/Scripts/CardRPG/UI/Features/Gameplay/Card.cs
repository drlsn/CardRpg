using CardRPG.UI.Features.Gameplay;
using Core.Functional;
using Core.Unity;
using Core.Unity.Scripts;
using Core.Unity.Transforms;
using Core.Unity.UI;
using Core.Unity.UI.Taps;
using System;
using System.Collections.Generic;
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

        [SerializeField] private Image _cardTypeIcon;

        [SerializeField] private Sprite _heroCardIcon;
        [SerializeField] private Sprite _unitCardIcon;
        [SerializeField] private Sprite _spellCardIcon;
        [SerializeField] private Sprite _skillCardIcon;
        [SerializeField] private Sprite _itemCardIcon;

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

            _cardTypeIcon.sprite = new Sprite[] { _unitCardIcon, _spellCardIcon, _skillCardIcon, _itemCardIcon }
                .GetRandom();

            int group = 1;
            //_image.sprite = _cardImages.GetAvers(1);
            //ReversedCardButton.GetComponent<Image>().sprite = _cardImages.GetRevers(0);
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

        public void SetDesc(string text) {}

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

        public void MoveTo(
            Vector2 targetPos,
            bool dontReverse = false,
            float cardMoveTime = 0.75f,
            MoveEffect effects = MoveEffect.None,
            Action onDone = null)
        {
            if (IsMoving)
                return;

            LerpFunctions.BeginLerp(RT, restore =>
            {
                LerpFunctions.LerpPosition2D(
                    StartCoroutine,
                    RT,
                    targetPos,
                    cardMoveTime,
                    onDone: () =>
                    {
                        IsMoving = false;
                        onDone?.Invoke();
                        restore();
                    });

                if ((effects & MoveEffect.Rotate) != 0)
                    LerpFunctions.LerpRotationZ(
                        StartCoroutine,
                        RT,
                        360,
                        cardMoveTime);

                if ((effects & MoveEffect.Scale2D) != 0)
                {
                    LerpFunctions.LerpScale2D(
                        StartCoroutine,
                        RT,
                        1.5f,
                        cardMoveTime / 2,
                        onDone: () =>
                        {
                            ReversedCardButton.SetActive(dontReverse);
                            //_descText.SetActive(false);
                            LerpFunctions.LerpScale2D(
                                StartCoroutine,
                                RT,
                                1f,
                                cardMoveTime / 2);
                        });
                }
                else
                if ((effects & MoveEffect.Scale3D) != 0)
                {
                    RunAsCoroutine(
                        () => LerpFunctions.LerpScaleX(
                            StartCoroutine,
                            RT,
                            0f,
                            cardMoveTime / 3,
                            onDone: () =>
                            {
                                ReversedCardButton.SetActive(dontReverse);
                                //_descText.SetActive(false);
                                LerpFunctions.LerpScaleX(
                                    StartCoroutine,
                                    RT,
                                    1f,
                                    cardMoveTime / 3);
                            }),
                        delaySeconds: cardMoveTime / 3);

                    RunAsCoroutine(
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
                                    cardMoveTime / 3)),
                            delaySeconds: cardMoveTime / 3);
                }
            });
        }

        [Flags]
        public enum MoveEffect
        {
            None = 0,
            Rotate = 1,
            Scale2D = 2,
            Scale3D = 4,
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
