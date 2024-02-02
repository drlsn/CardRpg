using CardRPG.Entities.Gameplay;
using CardRPG.Entities.Users;
using Core.Basic;
using System.Threading.Tasks;

namespace CardRPG.UseCases
{
    public class AttackCommandHandler
    {
        public async Task<Result<IDomainEvent[]>> Handle(AttackCommand cmd)
        {
            var game = StartRandomGameCommandHandler.Game;
            if (game is null)
                return false;

            var result = game.Attack(
                new UserId(cmd.AttackerId),
                new CardId(cmd.AttackerCardId),
                new CardId(cmd.DefenderCardId));

            //if (result.Value.Any(ev => ev is GameFinishedEvent))
            //    StartRandomGameCommandHandler.Game = null;

            return result;
        }
    }

    public class AttackCommand
    {
        public AttackCommand(
            string attackerId, 
            string attackerCardId,
            string defenderId,
            string defenderCardId)
        {
            AttackerId = attackerId;
            AttackerCardId = attackerCardId;
            DefenderId = defenderId;
            DefenderCardId = defenderCardId;
        }

        public string AttackerId { get; }
        public string AttackerCardId { get; }
        public string DefenderId { get; }
        public string DefenderCardId { get; }
    }
}
