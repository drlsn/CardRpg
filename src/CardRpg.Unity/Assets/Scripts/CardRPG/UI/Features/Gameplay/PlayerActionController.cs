using Core.Collections;
using Core.Unity.Popups;
using UnityEngine;

namespace CardRPG.UI.Gameplay
{
    public class PlayerActionController : MonoBehaviour
    {
        private Card[] _playerCards;
        private Card[] _enemyCards;

        public void Init(
            Card[] playerCards, Card[] enemyCards)
        {
            _playerCards = playerCards;
            _enemyCards = enemyCards;

            AssignOnCardSelected();
        }

        private void OnCardSelected(Entities.Gameplay.Card card, bool isEnemy)
        {
            var x = isEnemy ? "Enemy" : "Player";
            GameObject.FindAnyObjectByType<PopupController>().Show($"{x} card selected");
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
