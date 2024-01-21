using CardRPG.Entities.Gameplay;
using CardRPG.UI.GUICommands;
using CardRPG.UI.Infrastructure;
using CardRPG.UI.UseCases;
using CardRPG.UseCases;
using Core.Basic;
using Core.Collections;
using Core.Functional;
using Core.Unity;
using Core.Unity.Math;
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
        [SerializeField] private RectTransform _enemyBackRow;
        [SerializeField] private RectTransform _enemyHandRow;
        [SerializeField] private RectTransform _enemyBattleRow;
        private Card _enemyHero;

        // Player
        [SerializeField] private RectTransform _playerBattleRow;
        [SerializeField] private RectTransform _playerHandRow;
        [SerializeField] private RectTransform _playerBackRow;
        private Card _playerHero;

        [SerializeField] private RectTransform _middleRow;

        [SerializeField] private PlayerActionController _playerActionController;

        [SerializeField] private Card _cardPrefab;
        [SerializeField] private Card _cardBigPrefab;
        private Card _cardBig;

        private Card _myDeck;
        private Card _enemyDeck;
        [SerializeField] private Card _commonDeck;

        private IGameplayService _gameplayService;

        private RectTransform _rt;

        [SerializeField] private Image _dialogTreeBg;
        private DialogTree _dialogTree;

        public void Init(GetGameStateQueryOut dto)
        {
            _rt = this.RT();

            _gameplayService = new OfflineGameplayService(StartCoroutine);
            _gameplayService.Subscribe<EnemyCardTakenToHandEvent>(OnEnemyCardTakenToHandEvent);
            _gameplayService.Subscribe<EnemyCardLaidToBattleEvent>(OnEnemyCardLaidToBattleEvent);

            _dialogTree = new(_rt, _dialogTreeBg, StartCoroutine);

            Rebuild(dto);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                _dialogTree.Any()
                    .IfTrueDo(_dialogTree.Back)
                    .IfFalseDo(FindObjectOfType<GoToMenuGUICommand>().Execute);
        }

        private void OnEnemyCardTakenToHandEvent(EnemyCardTakenToHandEvent ev)
        {
            TakeCardToHand(onDone: null, forEnemy: true, ev.IsFromCommonDeck);
        }

        private void OnEnemyCardLaidToBattleEvent(EnemyCardLaidToBattleEvent ev)
        {
            var card = _enemyHandRow.GetComponentsInChildren<Card>().GetRandom();
            card.Init(ev.Card, isEnemy: true);

            LayCardToBattle(card, forEnemy: true, onDone: null);
        }

        public void Rebuild(GetGameStateQueryOut dto)
        {
            var steps = new ActionStepController();

            steps += Wait(0.5f);
            steps += SpawnHeroesAndDecks;
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

        private void SpawnHeroesAndDecks(Action onDone)
        {
            Card InitHero(RectTransform row, bool isLeft) =>
                   MoveInCard(row, isLeft, yOffset: (isLeft ? -1 : 1) * 5)
                       .Then(card => card.Turn(true))
                       .Then(card => card.AddCardDetailsOnTapHandler(_cardBigPrefab.RT, _dialogTree))
                       .Then(card => card.Init(new Entities.Gameplay.Card(null, OfflineGameplayService.Names.GetRandom(), null, -1), isLeft)); 

            // Heroes
            _playerHero = InitHero(_playerBackRow, isLeft: false);
            _enemyHero = InitHero(_enemyBackRow, isLeft: true);

            // Decks
            _myDeck = MoveInCard(_playerBackRow, isLeft: true, yOffset: 5, onDone).Then(card => card.Turn(false));
            _enemyDeck = MoveInCard(_enemyBackRow, isLeft: false, yOffset: -5).Then(card => card.Turn(false));
            _commonDeck.Turn(false);
        }

        private Card MoveInCard(RectTransform row, bool isLeft, float yOffset, Action onDone = null)
        {
            var card = _cardPrefab.Instantiate(row);

            var sign = isLeft ? -1 : 1;
            var xOffset = sign * (row.GetRTWidth() + card.RT.GetRTWidth()) / 2;
            var initPos = row.GetScreenCenterPos(xOffset, yOffset);
            card.RT.pivot = Vector2X.Half;
            card.RT.position = initPos;

            card.MoveTo(initPos.AddX(-sign * (card.RT.GetRTWidth() * card.RT.lossyScale.x + 10)), cardMoveTime: 0.75f, onDone: onDone);

            return card;
        }

        private void AnimateMixingCards(Action onDone)
        {
            var cardMoveTime = 0.75f;

            _myDeck.HideArrow();
            _enemyDeck.HideArrow();

            _myDeck.AnimateMixingCards(isMe: true, targetPos: _middleRow.GetScreenCenterPos(xOffset: -200), _commonDeck.RT, cardMoveTime);
            _enemyDeck.AnimateMixingCards(isMe: false, _middleRow.GetScreenCenterPos(xOffset: 200), _commonDeck.RT, cardMoveTime, 
                onDone: onDone);
        }

        private void StartTakeCardsToHand(Action onDone)
        {
            _myDeck.gameObject.SetActive(true);
            _enemyDeck.gameObject.SetActive(true);

            _commonDeck.gameObject.SetActive(true);
            _myDeck.ShowArrow();
            _commonDeck.ShowArrow();
            _enemyDeck.GrayOn();

            _myDeck.CardButton.OnSwipe(() => TakeCardToHand(onDone));
            _commonDeck.CardButton.OnSwipe(() => TakeCardToHand(onDone, fromCommonDeck: true));
        }

        public void TakeCardToHand(Action onDone, bool forEnemy = false, bool fromCommonDeck = false, float moveTime = 0.35f)
        {
            var row = forEnemy ? _enemyHandRow : _playerHandRow;

            var sourceCard = fromCommonDeck ? _commonDeck : (forEnemy ? _enemyDeck : _myDeck);
            Card card = null;
            MoveCardToRow(
                getCard: () => card = sourceCard.Instantiate().Then(card => card.Turn(false)), 
                row, Card.MoveEffect.Scale3D, moveTime, toAversOrRevers: !forEnemy, onDone: () =>
            {
                card.Init(new Entities.Gameplay.Card(null, OfflineGameplayService.Names.GetRandom(), null, -1), forEnemy);
                if (row.GetComponentsInChildren<Card>().Length == 6) 
                    (_myDeck + _commonDeck)
                        .ForEach(x => x.HideArrow().CardButton.RemoveHandlers())
                        .Then(onDone);
                
                if (!forEnemy)
                    card.AddCardDetailsOnTapHandler(_cardBigPrefab.RT, _dialogTree);
            });

            if (!forEnemy)
                _gameplayService.Send(new TakeCardToHandCommand("player-x", "card-x"));
        }

        private void StartLayingCardsToBattle(Action onDone)
        {
            _playerHandRow
                .GetChildren<Card>()
                .ForEach(card => card.CardButton
                    .Then(c => c.OnSwipe(() => LayCardToBattle(card, onDone: onDone))));
        }

        public void LayCardToBattle(Card card, bool forEnemy = false, float moveTime = 0.35f, Action onDone = null)
        {
            var row = forEnemy ? _enemyBattleRow : _playerBattleRow;
            if (!forEnemy)
                _gameplayService.Send(new LayCardToBattleCommand("player-x", "card-x"));

            var effects = !forEnemy ? Card.MoveEffect.Scale2D : Card.MoveEffect.Scale3D | Card.MoveEffect.Rotate;
            MoveCardToRow(() => card, row, effects, moveTime, toAversOrRevers: true, onDone: () =>
            {
                if (forEnemy)
                    card.AddCardDetailsOnTapHandler(_cardBigPrefab.RT, _dialogTree);
                onDone?.Invoke();
            });
        }

        public void MoveCardToRow(
            Func<Card> getCard,
            RectTransform row, 
            Card.MoveEffect effects, 
            float moveTime = 0.35f,
            bool? toAversOrRevers = null,
            Action onDone = null)
        { 
            var rowWidth = row.GetRTWidth();
            var cards = row.GetComponentsInChildren<Card>();
            var count = cards.Length;

            if (cards.Any(card => card.IsMoving) || cards.Length >= 6)
                return;

            var spacing = 20;
            cards.ForEach((card, i) =>
            {
                var offsetFactor = (float) (i - (count + 1) / 2f + 0.5f);
                var xOffset = offsetFactor * (card.RT.rect.width + spacing);
                var rowPos = row.GetScreenCenterPos(xOffset);

                card.MoveTo(rowPos, moveTime);
            });

            var card = getCard();
            var targetPos = row.RT().GetScreenCenterPos(xOffset: card.RT.rect.width * ((float) count / 2) + spacing * (count / 2f));

            card.RT.SetParent(row);
            card.CardButton.RemoveHandlers();
            UILayoutRebuilder.Rebuild(card.gameObject);
            card.GrayOff();
            card.MoveTo(targetPos, moveTime, effects, toAversOrRevers, onDone);
            count++;
        }
    }

    static class Extensions
    {
        public static void AddCardDetailsOnTapHandler(this Card card, RectTransform cardBigPrefab, DialogTree dialogTree) =>
            card.CardButton.OnTap(() => dialogTree.ShowDialog(cardBigPrefab, card.RT.GetScreenCenterPos()));
    }
}
