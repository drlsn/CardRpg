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

        private void AssignOnCardSelected()
        {
        }

        private void RemoveOnCardSelected()
        {
        }

        private void OnDestroy()
        {
            RemoveOnCardSelected();
        }
    }
}
