using CardRPG.Entities.Users;
using Core.Collections;
using System.Linq;

namespace CardRPG.Entities.Gameplay.Events
{
    public class AttackedEvent : _GameEvent
    {
        public AttackedEvent(
            GameId gameId,
            CombatantData attacker,
            CombatantData defender) : base(gameId, attacker.Id)
        {
            Attacker = attacker;
            Defender = defender;
        }

        public CombatantData Attacker { get; private set; }
        public CombatantData Defender { get; private set; }

        public override string ToString()
        {
            var attackText = Attacker.Cards.Length == 1 ? "attacks" : "attack";
            var attackerCardNames = Attacker.Cards.Select(c => c.Name).AggregateOrDefault((x, y) => $"{x}, {y}");
            var defenderCardTexts = Defender.Cards.Select((c, i) => 
            {
                if (i == 0)
                    return $"{c.Damage} damage to {c.Name}";

                return $"{c.Damage} to {c.Name}";
            }).AggregateOrDefault((x, y) => $"{x}, {y}");

            var enemiesText = Attacker.Cards.Length == 1 ? "" : "enemies ";

            return $"{attackerCardNames} {attackText} {enemiesText}inflicting {defenderCardTexts}";
        }

        public class CombatantData
        {
            public CombatantData(UserId id, string name, CardData[] cards)
            {
                Id = id;
                Name = name;
                Cards = cards;
            }

            public UserId Id { get; private set; }
            public string Name { get; private set; }
            public CardData[] Cards { get; private set; }
        }

        public class CardData
        {
            public CardData(CardId id, string name, int? damage = null)
            {
                Id = id;
                Name = name;
                Damage = damage;
            }

            public CardId Id { get; private set; }
            public string Name { get; private set; }
            public int? Damage { get; private set; }
        }
    }
}
