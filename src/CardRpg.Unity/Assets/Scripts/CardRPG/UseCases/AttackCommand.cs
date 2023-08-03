using CardRPG.Entities.Gameplay;
using CardRPG.Entities.Users;
using System.Threading.Tasks;

namespace CardRPG.UseCases
{
    public class AttackCommandHandler
    {
        public async Task Handle(AttackCommand cmd)
        {
            var game = StartRandomGameCommandHandler.Game;

            game.Attack(
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
