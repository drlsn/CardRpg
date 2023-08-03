using CardRPG.Entities.Users;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardRPG.Entities.Gameplay
{
    public class Game
    {
        public GameId Id { get; }
        public List<Player> Players { get; private set; }
        public TurnController TurnController { get; private set; }

        public bool IsGameOver { get; private set; }

        public Game(
            List<Player> players) : this(EntityId.New<GameId>(), players) {}

        public Game(
            GameId id,
            List<Player> players) 
        {
            Id = id;
            Players = players;
            TurnController = new(players.ToArray(), players.First().Id);
        }

        public bool Attack(UserId playerId, CardId sourceCardId, CardId targetCardId) =>
            Try(playerId, (thisPlayer, otherPlayer) =>
            {
                var sourceCard = thisPlayer.Cards.FirstOrDefault(c => c.Id == sourceCardId);
                var targetCard = otherPlayer.Cards.FirstOrDefault(c => c.Id == targetCardId);

                var damage = sourceCard.Statistics.Attack.CalculatedValue;
                targetCard.Statistics.HP.ModifyClamped(-damage);

                if (targetCard.Statistics.HP.CalculatedValue <= 0)
                    IsGameOver = true;

                return true;
            });

        public bool Try(UserId playerId, Func<Player, bool> action) =>
            TurnController.TryPerformTurn(playerId, player => IsGameOver ? false : action(player));

        public bool Try(UserId playerId, Func<Player, Player, bool> action) =>
            TurnController.TryPerformTurn(playerId, (thisPlayer, otherPlayer) => 
                IsGameOver ? false : action(thisPlayer, otherPlayer));
    }

    public class GameId : EntityId { public GameId(string value) : base(value) { } }
}
