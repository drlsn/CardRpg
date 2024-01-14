using CardRPG.UseCases;
using Core.Unity;
using Core.Unity.Popups;
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
            yield return new WaitForSeconds(1);

            var myDeck = Instantiate(_myDeck, _myDeck.transform.parent);
            var enemyDeck = Instantiate(_enemyDeck, _enemyDeck.transform.parent);

            _myDeck.gameObject.SetActive(false);
            _enemyDeck.gameObject.SetActive(false);

            enemyDeck.AnimateMixingCards(isMe: false);
            myDeck.AnimateMixingCards(isMe: true, onDoneFinal: () =>
            {
                _msg.HideMessage();
                
                _myDeck.gameObject.SetActive(true);
                _enemyDeck.gameObject.SetActive(true);

                _moveArea.DestroyChildren();

                _commonDeck.gameObject.SetActive(true);
            });

            yield return new WaitForSeconds(0.75f);
            _msg.Show("Mixing Cards");

        }
    }
}
