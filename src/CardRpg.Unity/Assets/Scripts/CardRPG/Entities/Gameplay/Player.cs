using CardRPG.Entities.Users;
using Core.Entities;
using System.Collections.Generic;

namespace CardRPG.Entities.Gameplay
{
    public class Player
    {
        public UserId Id { get; private set; }
        public List<Card> Cards { get; private set; } = new();

        public Player(
            List<Card> cards) : this(EntityId.New<UserId>(), cards) {}

        public Player(
            UserId id,
            List<Card> cards)
        {
            Id = id;
            Cards = cards;
        }
    }
}
