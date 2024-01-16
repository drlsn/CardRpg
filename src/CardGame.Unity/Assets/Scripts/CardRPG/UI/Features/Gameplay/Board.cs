using CardRPG.UseCases;
using Common.Unity.Coroutines;
using Core.Collections;
using Core.Functional;
using Core.Unity;
using Core.Unity.Math;
using Core.Unity.Popups;
using Core.Unity.UI;
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

            var enemyDeck = _enemyDeck.Instantiate(_enemyDeck.transform.parent);
            var myDeck = _myDeck.Instantiate(_myDeck.transform.parent);
            _enemyDeck.gameObject.SetActive(false);
            _myDeck.gameObject.SetActive(false);

            var cardMoveTime = 0.1f;// 0.75f;
            enemyDeck.AnimateMixingCards(isMe: false, _commonDeck.RT, cardMoveTime);
            myDeck.AnimateMixingCards(isMe: true, _commonDeck.RT, cardMoveTime, onDoneFinal: () =>
            {
                _msg.HideMessage();
                CoroutineExtensions.RunAsCoroutine(() => 
                {
                    _myDeck.gameObject.SetActive(true);
                    _enemyDeck.gameObject.SetActive(true);

                    _moveArea.DestroyChildren();

                    _commonDeck.gameObject.SetActive(true);

                    _myDeck.ShowArrow();
                    _commonDeck.ShowArrow();
                    _enemyDeck.GrayOn();

                    _myDeck.ReversedCardButton.onClick.AddListener(() => TakeCardToHand());
                    _commonDeck.ReversedCardButton.onClick.AddListener(() => TakeCardToHand(fromCommonDeck: true));

                }, 0.5f, StartCoroutine);

                CoroutineExtensions.RunAsCoroutine(() => _msg.Show("Take Cards"), 0.6f, StartCoroutine);
            });

            yield return new WaitForSeconds(cardMoveTime);

            _msg.Show("Mixing Cards");
        }

        public void TakeCardToHand(bool fromCommonDeck = false)
        {
            var row = _playerHandIO.Parent.RT();

            var rowWidth = row.GetRTWidth();
            var cards = row.GetComponentsInChildren<Card>();
            var count = cards.Length;
            
            var spacing = 20;
            cards.ForEach((card, i) =>
            {
                var offsetFactor = (float) (i - (count + 1) / 2f + 0.5f);
                var xOffset = offsetFactor * (card.RT.rect.width + spacing);
                var rowPos = row.GetScreenPos(xOffset);
                card.TranslateTo(rowPos);
            }); 

            var sourceCard = fromCommonDeck ? _commonDeck : _myDeck;
            var card = Instantiate(sourceCard, sourceCard.RT.position + sourceCard.RT.GetPivotOffset(Vector2Ex.Half), Quaternion.identity, row);
            card.RT.pivot = Vector2Ex.Half;
            card.RT.TranslateByWidthHalf();

            var targetPos = row.RT().GetScreenPos(xOffset: card.RT.rect.width * ((float) count / 2) + spacing * (count / 2f));
            card.MoveTo(targetPos, cardMoveTime: 0.75f);
            count++;

            if (count == 6)
                (_myDeck + _commonDeck)
                    .ForEach(x => x.HideArrow().ReversedCardButton.DisableAndRemoveListeners()); 
        }
    }
}
