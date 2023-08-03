using CardRPG.Entities.Users;

namespace CardRPG.Entities.Gameplay.Events
{
    public class GameFinishedEvent : _GameEvent
    {
        public GameFinishedEvent(GameId gameId, CombatantData winner) : base(gameId, winner.Id)
        {
            Winner = winner;
        }

        public CombatantData Winner { get; private set; }

        public override string ToString() =>
            $"Game finished. {Winner.Name} wins.";
        
        public class CombatantData
        {
            public CombatantData(UserId id, string name)
            {
                Id = id;
                Name = name;
            }

            public UserId Id { get; }
            public string Name { get; }
        }
    }
}
