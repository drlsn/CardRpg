using CardRPG.UseCases;
using Common.Unity.Coroutines;
using Core.Collections;
using Core.Functional;
using Core.Unity;
using Core.Unity.Functional;
using Core.Unity.Math;
using Core.Unity.Popups;
using Core.Unity.Scripts;
using Core.Unity.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Core.Unity.Functional.Delegates;

namespace CardRPG.UI.Gameplay
{
    public class Board : UnityScript
    {
        // Enemy
        [SerializeField] private CardRpgIOs.CardIOList _enemiesBackIO;
        [SerializeField] private CardRpgIOs.CardIOList _enemiesHandIO;
        [SerializeField] private CardRpgIOs.CardIOList _enemiesBattleIO;
        [SerializeField] private Card _enemyDeck;

        [SerializeField] private Card _commonDeck;

        // Player
        [SerializeField] private CardRpgIOs.CardIOList _playerBattleIO;
        [SerializeField] private CardRpgIOs.CardIOList _playerHandIO;
        [SerializeField] private CardRpgIOs.CardIOList _playerBackIO;
        [SerializeField] private Card _myDeck;

        [SerializeField] private PlayerActionController _playerActionController;
        [SerializeField] private RectTransform _moveArea;

        private MessagesController _msg;

        private void Awake()
        {
            _msg = GameObject.FindObjectOfType<MessagesController>();
        }

        public void Init(GetGameStateQueryOut dto)
        {
            Rebuild(dto);
        }

        public void Rebuild(GetGameStateQueryOut dto)
        {
            var steps = new ActionStepController();

            steps += onDone => _msg.Show("Mixing Cards").Then(onDone);
            steps += AnimateMixingCards;

            steps += StartTakeCardsToHand;

            RunAsCoroutine(steps.Execute);
        }

        public class ActionStepController
        {
            private List<Callback> _callbacks = new();
            private int _index;

            public static ActionStepController operator +(ActionStepController left, Callback right) =>
                right
                    .AddTo(left._callbacks)
                    .ThenReturn(left);

            public void Execute()
            {
                _index = 0;
                ExecuteNextCallback();
            }

            private void ExecuteNextCallback()
            {
                if (_index < _callbacks.Count)
                {
                    var currentCallback = _callbacks[_index];

                    Action onDone = () =>
                    {
                        _index++;
                        ExecuteNextCallback();
                    };

                    currentCallback(onDone);
                }
            }
        }

        private void AnimateMixingCards(Action onDone)
        {
            Debug.Log("AnimateMixingCards");
            var enemyDeck = _enemyDeck.Instantiate(_enemyDeck.transform.parent);
            var myDeck = _myDeck.Instantiate(_myDeck.transform.parent);
            _enemyDeck.gameObject.SetActive(false);
            _myDeck.gameObject.SetActive(false);

            var cardMoveTime = 0.75f;
            enemyDeck.AnimateMixingCards(isMe: false, _commonDeck.RT, cardMoveTime, onDone: onDone);
            myDeck.AnimateMixingCards(isMe: true, _commonDeck.RT, cardMoveTime);
        }

        private void StartTakeCardsToHand(Action onDone)
        {
            _msg.HideMessage();
            CoroutineExtensions.RunAsCoroutine(() =>
            {
                _myDeck.gameObject.SetActive(true);
                _enemyDeck.gameObject.SetActive(true);

                _moveArea.DestroyChildren();

                _commonDeck.gameObject.SetActive(true);

                _myDeck.ShowArrow();
                _commonDeck.ShowArrow();
                _enemyDeck.GrayOn();

                _myDeck.ReversedCardButton.onClick.AddListener(() => TakeCardToHand(onDone));
                _commonDeck.ReversedCardButton.onClick.AddListener(() => TakeCardToHand(onDone, fromCommonDeck: true));

            }, 0.5f, StartCoroutine);

            //CoroutineExtensions.RunAsCoroutine(() => _msg.Show("Take Cards"), 0.6f, StartCoroutine);
        }

        public void TakeCardToHand(Action onDone, bool fromCommonDeck = false)
        {
            var row = _playerHandIO.Parent.RT();

            var rowWidth = row.GetRTWidth();
            var cards = row.GetComponentsInChildren<Card>();
            var count = cards.Length;
            
            var spacing = 20;
            cards.ForEach((card, i) =>
            {
                var offsetFactor = (float) (i - (count + 1) / 2f + 0.5f);
                var xOffset = offsetFactor * (card.RT.rect.width + spacing);
                var rowPos = row.GetScreenPos(xOffset);
                card.TranslateTo(rowPos);
            }); 

            var sourceCard = fromCommonDeck ? _commonDeck : _myDeck;
            var card = Instantiate(sourceCard, sourceCard.RT.position + sourceCard.RT.GetPivotOffset(Vector2Ex.Half), Quaternion.identity, row);
            card.RT.pivot = Vector2Ex.Half;
            card.RT.TranslateByWidthHalf();

            var targetPos = row.RT().GetScreenPos(xOffset: card.RT.rect.width * ((float) count / 2) + spacing * (count / 2f));
            card.MoveTo(targetPos, cardMoveTime: 0.75f);
            count++;

            if (count == 6)
                (_myDeck + _commonDeck)
                    .ForEach(x => x.HideArrow().ReversedCardButton.DisableAndRemoveListeners())
                    .Then(onDone); 
        }
    }
}
