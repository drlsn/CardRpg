using CardRPG.UseCases;
using Core.Unity.Popups;
using Core.Unity.Transforms;
using System.Collections;
using UnityEngine;

namespace CardRPG.UI.Gameplay
{
    public class Board : MonoBehaviour
    {
        // Enemy
        [SerializeField] private CardRpgIOs.CardIOList _enemiesBackIO;
        [SerializeField] private CardRpgIOs.CardIOList _enemiesHandIO;
        [SerializeField] private CardRpgIOs.CardIOList _enemiesBattleIO;
        [SerializeField] private RectTransform _enemyDeck;

        // Player
        [SerializeField] private CardRpgIOs.CardIOList _playerBattleIO;
        [SerializeField] private CardRpgIOs.CardIOList _playerHandIO;
        [SerializeField] private CardRpgIOs.CardIOList _playerBackIO;
        [SerializeField] private RectTransform _myDeck;

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
            StartCoroutine(Move());

            //_enemiesBackIO.Destroy();
            //_enemiesHandIO.Destroy();
            //_enemiesBattleIO.Destroy();
            //_playerBattleIO.Destroy();
            //_playerHandIO.Destroy();
            //_playerBackIO.Destroy();

            //var player = dto.Game.Players.OfId(dto.PlayerId);
            //var enemy = dto.Game.Players.NotOfId(dto.PlayerId);

            //player.Cards.ForEach(card => _playerBackIO.Instantiate().Init(card, isEnemy: false));
            //enemy.Cards.ForEach(card => _enemiesBackIO.Instantiate().Init(card, isEnemy: true));

            //_playerActionController.Init(
            //    player.Id.Value,
            //    _playerBackIO.Objects.ToArray(),
            //    enemy.Id.Value,
            //    _enemiesBackIO.Objects.ToArray());
        }

        private IEnumerator Move()
        {
            _msg.Show("Mixing Cards");
            yield return new WaitForSeconds(2);

            LerpFunctions.LerpPosition2DComplex(
               _enemyDeck,
               _moveArea,
               new Vector2(Screen.width / 2 - _enemyDeck.rect.width - 10, Screen.height / 2),
               StartCoroutine);

            LerpFunctions.LerpPosition2DComplex(
               _myDeck,
               _moveArea,
               new Vector2(Screen.width / 2 + _myDeck.rect.width + 10, Screen.height / 2),
               StartCoroutine);
        }
    }
}
