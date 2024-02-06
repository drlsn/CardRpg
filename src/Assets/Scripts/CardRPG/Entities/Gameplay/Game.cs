using CardRPG.Entities.Gameplay.Events;
using CardRPG.Entities.Users;
using Core.Basic;
using Core.Collections;
using Core.Entities;
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

                var events = new List<IDomainEvent>();
                
                Add(new AttackedEvent(
                    gameId: Id,
                    attacker: new(thisPlayer.Id, thisPlayer.Name, new CardData[] { new(sourceCard.Id, sourceCard.Name) }),
                    defender: new(otherPlayer.Id, otherPlayer.Name, new CardData[] { new(targetCard.Id, targetCard.Name, (int) Math.Abs(resultDamage)) })))
                .AddTo(events);

                if (targetCard.Statistics.HP.CalculatedValue <= 0)
                {
                    Add(new GameFinishedEvent(
                        gameId: Id,
                        winner: new(thisPlayer.Id, thisPlayer.Name)))
                    .AddTo(events);

                    IsGameOver = true;
                }

                return new Result<IDomainEvent[]>(events.ToArray());
            });

        public Result<IDomainEvent[]> Try(UserId playerId, Func<Player, Result<IDomainEvent[]>> action) =>
            TurnController.TryPerformTurn(playerId, player => 
                IsGameOver ? Result<IDomainEvent[]>.Failure("Game is over") : action(player));

        public Result<IDomainEvent[]> Try(UserId playerId, Func<Player, Player, Result<IDomainEvent[]>> action) =>
            TurnController.TryPerformTurn(playerId, (thisPlayer, otherPlayer) => 
                IsGameOver ? Result<IDomainEvent[]>.Failure("Game is over") : action(thisPlayer, otherPlayer));
    }

    public class GameId : EntityId { public GameId(string value) : base(value) { } }
}
