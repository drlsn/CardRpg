using CardRPG.Entities.Gameplay;
using CardRPG.UI.UseCases;
using Common.Unity.Coroutines;
using Core.Collections;
using Core.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardRPG.UI.Infrastructure
{
    public class OfflineGameplayService : IGameplayService
    {
        private readonly Dictionary<Type, List<Action<IEvent>>> eventHandlers = new();
        private readonly Func<IEnumerator, Coroutine> _startCoroutine;

        private readonly List<string> _playerCardsInBattleRow = new();

        public OfflineGameplayService(Func<IEnumerator, Coroutine> startCoroutine) 
        {
            _startCoroutine = startCoroutine;
        }

        public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
        {
            var type = typeof(TEvent);
            List<Action<IEvent>> handlers = null;
            if (!eventHandlers.ContainsKey(type))
                eventHandlers.Add(typeof(TEvent), handlers = new());
            else 
                handlers = eventHandlers[type];

            handlers.Add(ev => handler((TEvent) ev));
        }

        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command is TakeCardToHandCommand)
                _startCoroutine(MakeEnemyReactToCardTake());

            if (command is LayCardToBattleCommand layCommand)
            {
                _playerCardsInBattleRow.Add(layCommand.CardId);
                if (_playerCardsInBattleRow.Count == 6)
                    6.ForEach(i => 
                        CoroutineExtensions.RunAsCoroutine(
                            MakeEnemyReactToPlayerCardsLaidToBattle, delaySeconds: i * 0.5f, _startCoroutine));
            }
        }

        private void RaiseEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var type = typeof(TEvent);
            if (!eventHandlers.ContainsKey(type))
                return;

            foreach (var handler in eventHandlers[typeof(TEvent)])
                handler(@event);
        }

        private IEnumerator MakeEnemyReactToCardTake()
        {
            yield return new WaitForSeconds(0.5f);
            RaiseEvent(new EnemyCardTakenToHandEvent(IsFromCommonDeck: RandomExtensions.GetRandomBool()));
        }

        private void MakeEnemyReactToPlayerCardsLaidToBattle()
        {
            RaiseEvent(new EnemyCardLaidToBattleEvent(
                new Card(new CardId(Guid.NewGuid().ToString()), Names.GetRandom(), null, -1)));
        }

        public static string[] Names = {
            "Elara Frost",
            "Thorne Shadow",
            "Lyra Swift",
            "Kael Ember",
            "Sorin Gale",
            "Lila Moon",
            "Zara Storm",
            "Finn Whisper",
            "Riven Hawk",
            "Nova Blade",
            "Kaius Thorn",
            "Selene Mist",
            "Vex Ironheart",
            "Evadne Frost",
            "Sylas Stone",
            "Myra Dawn",
            "Galen Ember",
            "Nia Sky",
            "Orion Frost",
            "Leona Storm",
            "Kieran Vale",
            "Seraph Night",
            "Elowen Frost",
            "Rune Star",
            "Talon Flame",
            "Zephyr Swift",
            "Selene Frost",
            "Kaela Mist",
            "Caspian Thorn",
            "Seraphina Frost"
        };
    }

    // Events
    public record EnemyCardTakenToHandEvent(
        bool IsFromCommonDeck) : IEvent;

    public record EnemyCardLaidToBattleEvent(
        Entities.Gameplay.Card Card) : IEvent;

    // Commands
    public record TakeCardToHandCommand(string PlayerId, string CardId) : ICommand;
    public record LayCardToBattleCommand(string PlayerId, string CardId) : ICommand;
}
