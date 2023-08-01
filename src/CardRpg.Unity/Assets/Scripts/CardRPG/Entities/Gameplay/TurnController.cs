using CardRPG.Entities.Users;
using System;
using System.Linq;
using UnityEngine.LowLevel;

namespace CardRPG.Entities.Gameplay
{
    public class TurnController
    {
        public Player[] Players { get; private set; }
        public UserId CurrentPlayer { get; private set; }
        public int CurrentTurn { get; private set; }
        public int TurnCount { get; private set; } = 1;

        public TurnController(
            Player[] players, UserId currentPlayer, int turnCount = 1)
        {
            Players = players;
            CurrentPlayer = currentPlayer;
            TurnCount = turnCount;
        }

        public bool SetActionDone(UserId userId)
        {
            if (!CanDo(userId))
                return false;

            if (CurrentTurn >= TurnCount)
            {
                CurrentPlayer = Players.First(player => player.Id != userId).Id;
                CurrentTurn = 0;
                TurnCount = 1;
            }

            return true;
        }

        public void UpdateTurnCount(int turnCount) => TurnCount = turnCount;

        public bool CanDo(UserId userId)
        {
            if (userId is null || 
                userId != CurrentPlayer)
            {
                return false;
            }

            return true;
        }

        public bool TryPerformTurn(UserId playerId, Func<Player, bool> action)
        {
            if (!CanDo(playerId))
                return false;

            var player = Players.OfId(playerId);
            if (!action(player))
                return false;

            return SetActionDone(playerId);
        }

        public bool TryPerformTurn(UserId playerId, Func<Player, Player, bool> action)
        {
            if (!CanDo(playerId))
                return false;

            var thisPlayer = Players.OfId(playerId);
            var otherPlayer = Players.NotOfId(playerId);
            if (!action(thisPlayer, otherPlayer))
                return false;

            return SetActionDone(playerId);
        }
    }
}
