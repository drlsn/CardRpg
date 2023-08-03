using CardRPG.Entities.Gameplay.Events;
using CardRPG.Entities.Users;
using Core.Basic;
using Core.Entities;
using Core.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using static CardRPG.Entities.Gameplay.Events.AttackedEvent;

namespace CardRPG.Entities.Gameplay
{
    public class Game : Entity<GameId>
    {
        public List<Player> Players { get; private set; }
        public TurnController TurnController { get; private set; }

        public bool IsGameOver { get; private set; }

        public Game(
            List<Player> players) : this(EntityId.New<GameId>(), players) {}

        public Game(
            GameId id,
            List<Player> players) : base(id)
        {
            Players = players;
            TurnController = new(players.ToArray(), players.First().Id);
        }

        public Result<IDomainEvent[]> Attack(UserId playerId, CardId sourceCardId, CardId targetCardId) =>
            Try(playerId, (thisPlayer, otherPlayer) =>
            {
                var sourceCard = thisPlayer.Cards.FirstOrDefault(c => c.Id == sourceCardId);
                var targetCard = otherPlayer.Cards.FirstOrDefault(c => c.Id == targetCardId);

                var damage = sourceCard.Statistics.Attack.CalculatedValue;
                targetCard.Statistics.HP.ModifyClamped(-damage, out double resultDamage);

                if (targetCard.Statistics.HP.CalculatedValue <= 0)
                    IsGameOver = true;

                return Add(new AttackedEvent(
                    attacker: new(thisPlayer.Id, thisPlayer.Id.Value, new CardData[] { new(sourceCard.Id, sourceCard.Name) }),
                    defender: new(otherPlayer.Id, otherPlayer.Id.Value, new CardData[] { new(targetCard.Id, targetCard.Name, (int) Math.Abs(resultDamage)) })))
                .ToArray();
            });

        public Result<IDomainEvent[]> Try(UserId playerId, Func<Player, Result<IDomainEvent[]>> action) =>
            TurnController.TryPerformTurn(playerId, player => IsGameOver ? false : action(player));

        public Result<IDomainEvent[]> Try(UserId playerId, Func<Player, Player, Result<IDomainEvent[]>> action) =>
            TurnController.TryPerformTurn(playerId, (thisPlayer, otherPlayer) => 
                IsGameOver ? false : action(thisPlayer, otherPlayer));
    }

    public class GameId : EntityId { public GameId(string value) : base(value) { } }
}
