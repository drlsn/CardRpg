using CardRPG.Entities.Gameplay.Events;
using CardRPG.UI.GUICommands;
using CardRPG.UseCases;
using Core.Collections;
using Core.Unity.Popups;
using Core.Unity.UI;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

        private Board _board;

        private DeviceOrientation _orientation;
        private Vector2 _lastScreenSize;

        private GameObject _backRow;

        private void Start()
        {
            _msg = GameObject.FindAnyObjectByType<MessagesController>();
            _board = FindAnyObjectByType<Board>();
            _orientation = Input.deviceOrientation;
            _lastScreenSize = new Vector2(Screen.width, Screen.height);
            _backRow = GameObject.FindGameObjectWithTag("BackRow");
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

        private void Update()
        {
            var screenSize = new Vector2(Screen.width, Screen.height);
            //if (Input.deviceOrientation != _orientation || _lastScreenSize != screenSize)
            //{
            //    _orientation = Input.deviceOrientation;
            //    _lastScreenSize = screenSize;

            //    var fitters = GameObject.FindObjectsOfType<AspectRatioFitter>();
            //    fitters.ForEach(f =>
            //    {
            //        var parentLayout = f.transform.parent.GetComponent<HorizontalLayoutGroup>();
            //        if (parentLayout is null)
            //            return;

            //        if (_orientation == DeviceOrientation.Portrait || screenSize.x < screenSize.y)
            //        {
            //            f.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
            //            parentLayout.childControlWidth = true;
            //            parentLayout.childForceExpandWidth = true;
            //            parentLayout.childForceExpandHeight = false;
            //            parentLayout.childControlHeight = false;
            //            _backRow.SetActive(true);
            //        }
            //        else
            //        {
            //            f.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
            //            parentLayout.childControlHeight = true;
            //            parentLayout.childForceExpandHeight = true;
            //            parentLayout.childControlWidth = false;
            //            parentLayout.childForceExpandWidth = false;
            //            _backRow.SetActive(false);
            //        }
            //    });

            //    _msg.Show(_orientation.ToString());
            //}
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
                    _board.Rebuild(dto);

                    attackResult.Value.ForEach(ev => _msg.Show(ev.ToString()));

                    if (attackResult.Value.Any(ev => ev is GameFinishedEvent))
                    {
                        _msg.Show("Leaving to menu in 3s...");
                        StartRandomGameCommandHandler.Game = null;
                        _board.SetInteractable(false);
                        GameObject.FindObjectOfType<GoToMenuGUICommand>().Execute();
                    }
                }
            }

            _lastSelectedCard = card;
            _isLastSelectedCardEnemy = isEnemy;
        }

        private void AssignOnCardSelected()
        {
            RemoveOnCardSelected();

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
