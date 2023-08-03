using CardRPG.Entities.Users;
using Core.Entities;
using System.Collections.Generic;

namespace CardRPG.Entities.Gameplay
{
    public class Player
    {
        public UserId Id { get; private set; }
        public List<Card> Cards { get; private set; } = new();
        public string Name { get; private set; }

        public Player(
            string name,
            List<Card> cards) : this(EntityId.New<UserId>(), cards)
        {
            Name = name;
        }

        public Player(
            UserId id,
            List<Card> cards)
        {
            Id = id;
            Cards = cards;
        }
    }
}
