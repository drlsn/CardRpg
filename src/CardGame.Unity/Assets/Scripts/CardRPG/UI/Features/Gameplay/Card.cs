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
using System;
using CardRPG.UI.Features.Gameplay;
using Core.Unity;
using System.Diagnostics.Tracing;
using Core.Basic;
using Common.Unity.Coroutines;
using System.Collections.Specialized;

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

        private void Awake()
        {
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

        public void ShowArrow() => _arrowController.Show();
        public void HideArrow() => _arrowController.Hide();

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
            float timeOffsetFactor = 0.66f,
            int cardCount = 7,
            Action onDoneFinal = null)
        {
            var initialPos = RT.position;

            var targetPos = _moveArea.GetScreenPos(xOffset: RT.rect.width * 2 * (isMe ? -1 : 1));
             
            LerpFunctions.BeginLerp(RT, _moveArea, onDone =>
            {
                LerpFunctions.LerpPosition2D(   
                    StartCoroutine,
                    RT,
                    targetPos,
                    cardMoveTime,
                    onDone: AnimateCascade);

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

            void AnimateCascade()
            {
                var totalTime = cardCount * mixCardMoveTime * timeOffsetFactor;

                if (isMe)
                    AnimateDeckCascade(ScreenEx.Middle, mixCardMoveTime, timeOffsetFactor, cardCount);
                else
                    StartCoroutine(CRT());

                StartCoroutine(MoveOwnDecksBack(totalTime));

                IEnumerator CRT()
                {
                    yield return new WaitForSeconds(mixCardMoveTime / 2);
                    AnimateDeckCascade(ScreenEx.Middle, mixCardMoveTime, timeOffsetFactor, cardCount);
                }
            }

            IEnumerator MoveOwnDecksBack(float waitTime)
            {
                yield return new WaitForSeconds(waitTime);

                LerpFunctions.BeginLerp(RT, _moveArea, onDone =>
                {
                    LerpFunctions.LerpPosition2D(
                        StartCoroutine,
                        RT,
                        initialPos,
                        cardMoveTime,
                        onDone: () =>
                        {
                            isMe.IfFalseDo(() => _reversedCardsIO.Destroy());
                            isMe.IfTrueDo(MoveCommonDeckBack);
                        });

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

                LerpFunctions.BeginLerp(card.RT, _moveArea, onDone =>
                {
                    LerpFunctions.LerpPosition2D(
                        StartCoroutine,
                        card.RT,
                        commonDeckTarget.GetScreenPos(),
                        cardMoveTime,
                        onDone: onDoneFinal);
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
                RTPresetsExtensions.StretchToMiddle(card);

                LerpFunctions.BeginLerp(card.RT, _moveArea, onDone =>
                {
                    LerpFunctions.LerpPosition2D(
                        StartCoroutine,
                        card.RT,
                        targetPos,
                        durationSeconds: cardMoveTime);
                });
            }
        }

        public void TranslateTo(
            Vector2 targetPos,
            float cardMoveTime = 0.75f)
        {
            var delaySeconds = 0f;
            if (_moveOrderTimes.Count > 0)
                delaySeconds = (float) ((DateTime.UtcNow.Ticks - (long) _moveOrderTimes[_moveOrderTimes.Count - 1]) / 1_000_000);
            Debug.Log($"seconds: {delaySeconds}");
            CoroutineExtensions.RunAsCoroutine(() => 
            {
                LerpFunctions.LerpPosition2D(
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
            RectTransform target,
            float cardMoveTime = 0.75f)
        {
            _moveOrderCounter.Increase();

            var timeId = Guid.NewGuid().ToString();
            _moveOrderTimes.Add(timeId, DateTime.UtcNow.Ticks);

            RT.pivot = Vector2.one / 2;
            LerpFunctions.LerpPosition2D(
                StartCoroutine,
                RT,
                target.GetScreenPos(),
                cardMoveTime,
                onDone: () =>
                {
                    _moveOrderCounter.Decrease();
                    _moveOrderTimes.Remove(timeId);
                });

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
