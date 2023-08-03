using CardRPG.Entities.Gameplay;
using CardRPG.UseCases;
using System.Linq;
using UnityEngine;

namespace CardRPG.UI.Gameplay
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private CardRpgIOs.CardIOList _enemiesBackIO;
        [SerializeField] private CardRpgIOs.CardIOList _enemiesFrontIO;

        [SerializeField] private CardRpgIOs.CardIOList _playerBackIO;
        [SerializeField] private CardRpgIOs.CardIOList _playerFrontIO;

        [SerializeField] private PlayerActionController _playerActionController;

        public void Init(GetGameStateQueryOut dto)
        {
            var player = dto.Game.Players.OfId(dto.PlayerId);
            var enemy = dto.Game.Players.NotOfId(dto.PlayerId);

            player.Cards.ForEach(card => _playerBackIO.Instantiate().Init(card, isEnemy: false));
            enemy.Cards.ForEach(card => _enemiesBackIO.Instantiate().Init(card, isEnemy: true));

            _playerActionController.Init(
                _playerBackIO.Objects.ToArray(),
                _enemiesBackIO.Objects.ToArray());
        }
    }
}
