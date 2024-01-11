using CardRPG.Entities.Gameplay;
using CardRPG.UI.GUICommands;
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
            Rebuild(dto);
        }

        public void Rebuild(GetGameStateQueryOut dto)
        {
            _enemiesBackIO.Destroy();
            _enemiesFrontIO.Destroy();
            _playerBackIO.Destroy();
            _playerFrontIO.Destroy();

            var player = dto.Game.Players.OfId(dto.PlayerId);
            var enemy = dto.Game.Players.NotOfId(dto.PlayerId);

            player.Cards.ForEach(card => _playerBackIO.Instantiate().Init(card, isEnemy: false));
            enemy.Cards.ForEach(card => _enemiesBackIO.Instantiate().Init(card, isEnemy: true));

            _playerActionController.Init(
                player.Id.Value,
                _playerBackIO.Objects.ToArray(),
                enemy.Id.Value,
                _enemiesBackIO.Objects.ToArray());
        }
    }
}
