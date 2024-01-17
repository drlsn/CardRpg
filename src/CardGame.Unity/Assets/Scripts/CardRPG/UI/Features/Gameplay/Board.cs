using CardRPG.UI.Infrastructure;
using CardRPG.UI.UseCases;
using CardRPG.UseCases;
using Core.Basic;
using Core.Collections;
using Core.Functional;
using Core.Unity;
using Core.Unity.Scripts;
using Core.Unity.UI;
using System;
using System.Linq;
using UnityEngine;

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

        private IGameplayService _gameplayService;

        public void Init(GetGameStateQueryOut dto)
        {
            _gameplayService = new OfflineGameplayService(StartCoroutine);
            _gameplayService.Subscribe<CardTakenToHandEvent>(On);

            Rebuild(dto);
        }

        private void On(CardTakenToHandEvent ev)
        {
            TakeCardToHand(onDone: null, forEnemy: true, ev.IsFromCommonDeck);
        }

        public void Rebuild(GetGameStateQueryOut dto)
        {
            var steps = new ActionStepController();

            steps += Show("Mixing Cards");
            steps += AnimateMixingCards;
            //steps += Wait(0.5f);
            steps += Show("Take Cards");
            steps += StartTakeCardsToHand;

            RunAsCoroutine(steps.Execute);
        }

        private void AnimateMixingCards(Action onDone)
        {
            var enemyDeck = _enemyDeck.Instantiate(_enemyDeck.transform.parent);
            var myDeck = _myDeck.Instantiate(_myDeck.transform.parent);

            _enemyDeck.gameObject.SetActive(false);
            _myDeck.gameObject.SetActive(false);

            var cardMoveTime = 0.75f;
            myDeck.AnimateMixingCards(isMe: true, _commonDeck.RT, cardMoveTime);
            enemyDeck.AnimateMixingCards(isMe: false, _commonDeck.RT, cardMoveTime, onDone: 
                onDone.Then(enemyDeck.Destroy).Then(myDeck.Destroy));
        }

        private void StartTakeCardsToHand(Action onDone)
        {
            ///_msg.HideMessage();

            _myDeck.gameObject.SetActive(true);
            _enemyDeck.gameObject.SetActive(true);

            _moveArea.DestroyChildren();

            _commonDeck.gameObject.SetActive(true);

            _myDeck.ShowArrow();
            _commonDeck.ShowArrow();
            _enemyDeck.GrayOn();

            _myDeck.ReversedCardButton.onClick.AddListener(() => TakeCardToHand(onDone));
            _commonDeck.ReversedCardButton.onClick.AddListener(() => TakeCardToHand(onDone, fromCommonDeck: true));
        }

        public void TakeCardToHand(Action onDone, bool forEnemy = false, bool fromCommonDeck = false)
        {
            var row = forEnemy ? _enemiesHandIO.Parent.RT() : _playerHandIO.Parent.RT();

            var rowWidth = row.GetRTWidth();
            var cards = row.GetComponentsInChildren<Card>();
            var count = cards.Length;

            if (cards.Any(card => card.IsMoving))
                return;

            var spacing = 20;
            cards.ForEach((card, i) =>
            {
                var offsetFactor = (float)(i - (count + 1) / 2f + 0.5f);
                var xOffset = offsetFactor * (card.RT.rect.width + spacing);
                var rowPos = row.GetScreenPos(xOffset);
                card.TranslateTo(rowPos);
            });

            var sourceCard = fromCommonDeck ? _commonDeck : (forEnemy ? _enemyDeck : _myDeck);
            var card = sourceCard.Instantiate(row);
            card.GrayOff();

            var targetPos = row.RT().GetScreenPos(xOffset: card.RT.rect.width * ((float) count / 2) + spacing * (count / 2f));
            card.MoveTo(targetPos, cardMoveTime: 0.35f);
            count++;

            _gameplayService.Send(new TakeCardToHandCommand(!forEnemy));
            if (count == 6)
                (_myDeck + _commonDeck)
                    .ForEach(x => x.HideArrow().ReversedCardButton.DisableAndRemoveListeners())
                    .Then(onDone); 
        }
    }
}
