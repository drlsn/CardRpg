using CardRPG.Entities.Gameplay;
using System.Linq;
using System.Reflection;

namespace CardRPG.UseCases
{
    internal class DefaultCards
    {
        public static readonly Card[] All;

        static DefaultCards()
        {
            All = typeof(DefaultCards)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => f.GetValue(null))
                .OfType<Card>()
                .ToArray();
        }

        public static Card Zawisza = new Card(
            new CardId("Zawisza"),
            "Zawisza Czarny",
            new StatisticPointGroup(
                hp: new StatisticPoint(10),
                attack: new StatisticPoint(4)));

        public static Card Ulryk = new Card(
            new CardId("Ulryk"),
            "Ulryk Von Jungingen",
            new StatisticPointGroup(
                hp: new StatisticPoint(10),
                attack: new StatisticPoint(3)));
    }
}
