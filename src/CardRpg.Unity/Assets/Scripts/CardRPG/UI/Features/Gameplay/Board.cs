using CardRPG.Entities.Gameplay;
using CardRPG.UseCases;
using UnityEngine;

namespace CardRPG.UI.Gameplay
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private Transform _enemiesBack;
        [SerializeField] private Transform _enemiesFront;

        [SerializeField] private Transform _playerBack;
        [SerializeField] private Transform _playerFront;

        [SerializeField] private CardRpgIOs.CardIOList _cardIO;

        public void Init(GetGameStateQueryOut dto)
        {
            var player = dto.Game.Players.OfId(dto.PlayerId);
            var enemy = dto.Game.Players.NotOfId(dto.PlayerId);

            SpawnCards(player, _playerBack);
            SpawnCards(enemy, _enemiesBack);
        }

        private void SpawnCards(Player player, Transform parent)
        {
            player.Cards.ForEach(card =>
            {
                var cardUI = _cardIO.Instantiate();
                cardUI.Init(card);
                cardUI.transform.SetParent(parent);
            });
        }
    }
}
