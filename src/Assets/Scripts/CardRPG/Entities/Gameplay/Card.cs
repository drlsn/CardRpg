using Core.Entities;

namespace CardRPG.Entities.Gameplay
{
    public class Card
    {
        public CardId Id { get; }
        public string Name { get; }
        public StatisticPointGroup Statistics { get; }
        public int ImageIndex { get; }

        public Card(
            CardId id,
            string name,
            StatisticPointGroup statistics,
            int imageIndex)
        {
            Id = id;
            Name = name;
            Statistics = statistics;
            ImageIndex = imageIndex;
        }

        public Card DeepCopy() =>
            new(new CardId(Id.Value),
                Name, Statistics.DeepCopy(), ImageIndex);
    }

    public class CardId : EntityId { public CardId(string value) : base(value) { } }
}
