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
                .Take(12)
                .Select((imgIdx, i) =>
                    i switch
                    {
                        0 => Zawisza(imgIdx),
                        1 => Ulryk(imgIdx),
                        2 => Msciwoj(imgIdx),
                        3 => Godryk(imgIdx),
                        4 => Jerzy(imgIdx),
                        5 => Siemko(imgIdx),
                        6 => Jaca(imgIdx),
                        7 => Milko(imgIdx),
                        8 => Janek(imgIdx),
                        9 => Kokosz(imgIdx),
                        10 => Kajko(imgIdx),
                        11 => Miko(imgIdx),
                        _ => Zawisza(imgIdx),
                    })
                .ToArray();

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

        private static Card Godryk(int imgIdx) => new Card(
            new CardId("Godryk"),
            "Godryk",
            new StatisticPointGroup(
                hp: new StatisticPoint(10),
                attack: new StatisticPoint(3)),
                imgIdx);

        private static Card Msciwoj(int imgIdx) => new Card(
            new CardId("Msciwoj"),
            "Msciwoj",
            new StatisticPointGroup(
                hp: new StatisticPoint(10),
                attack: new StatisticPoint(3)),
                imgIdx);

        private static Card Jerzy(int imgIdx) => new Card(
            new CardId("Jerzy"),
            "Jerzy",
            new StatisticPointGroup(
                hp: new StatisticPoint(10),
                attack: new StatisticPoint(3)),
                imgIdx);

        private static Card Siemko(int imgIdx) => new Card(
            new CardId("Siemko"),
            "Siemko",
            new StatisticPointGroup(
                hp: new StatisticPoint(10),
                attack: new StatisticPoint(3)),
                imgIdx);

        private static Card Jaca(int imgIdx) => new Card(
           new CardId("Jaca"),
           "Jaca",
           new StatisticPointGroup(
               hp: new StatisticPoint(10),
               attack: new StatisticPoint(3)),
               imgIdx);

        private static Card Milko(int imgIdx) => new Card(
           new CardId("Milko"),
           "Milko",
           new StatisticPointGroup(
               hp: new StatisticPoint(10),
               attack: new StatisticPoint(3)),
               imgIdx);

        private static Card Janek(int imgIdx) => new Card(
           new CardId("Janek"),
           "Janek",
           new StatisticPointGroup(
               hp: new StatisticPoint(10),
               attack: new StatisticPoint(3)),
               imgIdx);

        private static Card Kokosz(int imgIdx) => new Card(
           new CardId("Kokosz"),
           "Kokosz",
           new StatisticPointGroup(
               hp: new StatisticPoint(10),
               attack: new StatisticPoint(3)),
               imgIdx);

        private static Card Kajko(int imgIdx) => new Card(
           new CardId("Kajko"),
           "Kajko",
           new StatisticPointGroup(
               hp: new StatisticPoint(10),
               attack: new StatisticPoint(3)),
               imgIdx);

        private static Card Miko(int imgIdx) => new Card(
           new CardId("Miko"),
           "Miko",
           new StatisticPointGroup(
               hp: new StatisticPoint(10),
               attack: new StatisticPoint(3)),
               imgIdx);
    }
}
