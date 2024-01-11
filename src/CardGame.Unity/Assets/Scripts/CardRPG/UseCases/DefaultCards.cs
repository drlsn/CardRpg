using CardRPG.Entities.Gameplay;
using Core.Collections;
using System;
using System.Linq;

namespace CardRPG.UseCases
{
    internal class DefaultCards
    {
        public static Card[] All => 
            Enumerable
                .Range(0, 14)
                .Shuffle()
                .Take(2)
                .Select((imgIdx, i) =>
                    i switch
                    {
                        0 => Zawisza(imgIdx),
                        1 => Ulryk(imgIdx),
                        _ => Zawisza(imgIdx),
                    })
                .ToArray();

        //static DefaultCards()
        //{
        //    All = typeof(DefaultCards)
        //        .GetFields(BindingFlags.Public | BindingFlags.Static)
        //        .Select(f => f.GetValue(null))
        //        .OfType<Card>()
        //        .ToArray();
        //}

        private static readonly Random Random = new Random();

        private static Card Zawisza(int imgIdx) => new Card(
            new CardId("Zawisza"),
            "John",
            new StatisticPointGroup(
                hp: new StatisticPoint(10),
                attack: new StatisticPoint(4)),
                imgIdx);

        private static Card Ulryk(int imgIdx) => new Card(
            new CardId("Ulryk"),
            "James",
            new StatisticPointGroup(
                hp: new StatisticPoint(10),
                attack: new StatisticPoint(3)),
                imgIdx);
    }
}
