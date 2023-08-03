using CardRPG.Entities.Gameplay.Events;
using CardRPG.UseCases;
using Core.Collections;
using Core.Unity.Popups;
using Core.Unity.UI;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardRPG.UI.Gameplay
{
    public class PlayerActionController : MonoBehaviour
    {
        private string _playerId;
        private Card[] _playerCards;

        private string _enemyId;
        private Card[] _enemyCards;

        private Entities.Gameplay.Card _lastSelectedCard;
        private bool _isLastSelectedCardEnemy;

        private MessagesController _msg;

        private void Start()
        {
            _msg = GameObject.FindAnyObjectByType<MessagesController>();
        }

        public void Init(
            string playerId,
            Card[] playerCards,
            string enemyId,
            Card[] enemyCards)
        {
            _playerId = playerId;
            _playerCards = playerCards;

            _enemyId = enemyId;
            _enemyCards = enemyCards;

            AssignOnCardSelected();
        }

        private async void OnCardSelected(Entities.Gameplay.Card card, bool isEnemy)
        {
            if (isEnemy && !_isLastSelectedCardEnemy && _lastSelectedCard is not null)
            {
                var attackResult = await new AttackCommandHandler().Handle(
                    new AttackCommand(_playerId, _lastSelectedCard.Id.Value, _enemyId, card.Id.Value));

                if (!attackResult.IsSuccess)
                {
                    _msg.Show("Attack failed");
                }
                else
                {
                    var dto = await new GetGameStateQueryHandler().Handle(new GetGameStateQuery());
                    var board = FindAnyObjectByType<Board>();
                    board.Rebuild(dto);

                    attackResult.Value.ForEach(ev => _msg.Show(ev.ToString()));

                    if (attackResult.Value.Any(ev => ev is GameFinishedEvent))
                    {
                        _msg.Show("Leaving to menu in 3s...");
                        StartRandomGameCommandHandler.Game = null;
                        board.SetInteractable(false);
                        StartCoroutine(LeaveToMenu());
                    }
                }
            }

            _lastSelectedCard = card;
            _isLastSelectedCardEnemy = isEnemy;
        }

        private IEnumerator LeaveToMenu()
        {
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(0);
        }

        private void AssignOnCardSelected()
        {
            AssignOnCardSelected(_playerCards);
            AssignOnCardSelected(_enemyCards);
        }

        private void RemoveOnCardSelected()
        {
            RemoveOnCardSelected(_playerCards);
            RemoveOnCardSelected(_enemyCards);
        }

        private void AssignOnCardSelected(Card[] cards) =>
            cards.ForEach(card => card.OnCardSelected += OnCardSelected);

        private void RemoveOnCardSelected(Card[] cards) =>
           cards.ForEach(card => card.OnCardSelected -= OnCardSelected);

        private void OnDestroy()
        {
            RemoveOnCardSelected();
        }
    }
}
