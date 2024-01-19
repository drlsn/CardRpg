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
using UnityEngine.UI;

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
            steps += Wait(0.5f);
            steps += Show("You Strike First", 1f);
            steps += Show("Lay the Cards");
            steps += StartLayingCardsToBattle;

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
            _myDeck.gameObject.SetActive(true);
            _enemyDeck.gameObject.SetActive(true);

            _moveArea.DestroyChildren();

            _commonDeck.gameObject.SetActive(true);

            _myDeck.ShowArrow();
            _commonDeck.ShowArrow();
            _enemyDeck.GrayOn();

            _myDeck.CardButton.OnSwipe(() => TakeCardToHand(onDone));
            _commonDeck.CardButton.OnSwipe(() => TakeCardToHand(onDone, fromCommonDeck: true));
        }

        public void TakeCardToHand(Action onDone, bool forEnemy = false, bool fromCommonDeck = false, float moveTime = 0.35f)
        {
            var row = forEnemy ? _enemiesHandIO.Parent.RT() : _playerHandIO.Parent.RT();

            var sourceCard = fromCommonDeck ? _commonDeck : (forEnemy ? _enemyDeck : _myDeck);
            var card = sourceCard.Instantiate();

            MoveCardToRow(card, row, Card.MoveEffect.Scale3D, moveTime, onDone: () =>
            {
                (_myDeck + _commonDeck)
                    .ForEach(x => x.HideArrow().CardButton.RemoveHandlers())
                    .Then(onDone);
            });

            _gameplayService.Send(new TakeCardToHandCommand(!forEnemy));
        }

        private void StartLayingCardsToBattle(Action onDone)
        {
            _playerHandIO.Parent
                .GetChildren<Card>()
                .ForEach(card => card.CardButton
                    .Then(c => c.OnSwipe(() => LayCardToBattle(card, onDone: onDone))));
        }

        public void LayCardToBattle(Card card, bool forEnemy = false, float moveTime = 0.35f, Action onDone = null)
        {
            var row = forEnemy ? _enemiesBattleIO.Parent.RT() : _playerBattleIO.Parent.RT();
            MoveCardToRow(card, row, Card.MoveEffect.Scale2D, moveTime, onDone);
        }

        public void MoveCardToRow(
            Card card, RectTransform row, Card.MoveEffect effects, float moveTime = 0.35f,  Action onDone = null)
        { 
            var rowWidth = row.GetRTWidth();
            var cards = row.GetComponentsInChildren<Card>();
            var count = cards.Length;

            var spacing = 20;
            cards.ForEach((card, i) =>
            {
                var offsetFactor = (float) (i - (count + 1) / 2f + 0.5f);
                var xOffset = offsetFactor * (card.RT.rect.width + spacing);
                var rowPos = row.GetScreenPos(xOffset);

                card.MoveTo(rowPos, moveTime);
            });

            var targetPos = row.RT().GetScreenPos(xOffset: card.RT.rect.width * ((float) count / 2) + spacing * (count / 2f));
            card.RT.SetParent(row);
            card.CardButton.RemoveHandlers();
            UILayoutRebuilder.Rebuild(card.gameObject);
            card.GrayOff();
            card.MoveTo(targetPos, moveTime, effects);
            count++;

            if (count == 6)
                row
                   .GetChildren<Card>()
                   .ForEach(card => card.CardButton.RemoveHandlers())
                   .Then(onDone);
        }
    }
}
