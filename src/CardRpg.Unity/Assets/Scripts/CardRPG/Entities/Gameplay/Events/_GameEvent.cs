using CardRPG.Entities.Users;

namespace CardRPG.Entities.Gameplay.Events
{
    public class _GameEvent : IDomainEvent
    {
        public _GameEvent(
            GameId gameId,
            UserId initiatorId = null)
        {
            GameId = gameId;
            InitiatorId = initiatorId;
        }

        public GameId GameId { get; }
        public UserId InitiatorId { get; }
    }
}
