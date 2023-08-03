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

            return game.Attack(
                new UserId(cmd.AttackerId),
                new CardId(cmd.AttackerCardId),
                new CardId(cmd.DefenderCardId));
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
