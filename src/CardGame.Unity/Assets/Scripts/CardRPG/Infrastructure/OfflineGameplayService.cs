using CardRPG.UI.UseCases;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardRPG.UI.Infrastructure
{
    public class OfflineGameplayService : IGameplayService
    {
        private readonly List<Action<IEvent>> eventHandlers = new List<Action<IEvent>>();
        private readonly Func<IEnumerator, Coroutine> _startCoroutine;

        public OfflineGameplayService(Func<IEnumerator, Coroutine> startCoroutine) 
        {
            _startCoroutine = startCoroutine;
        }

        public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
        {
            eventHandlers.Add(e => handler((TEvent)e));
        }

        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command is TakeCardToHandCommand cmd && cmd.isPlayer)
                _startCoroutine(MakeEnemyReactToCardTake());
        }

        private void RaiseEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            foreach (var handler in eventHandlers)
                handler(@event);
        }

        private IEnumerator MakeEnemyReactToCardTake()
        {
            yield return new WaitForSeconds(0.5f);
            RaiseEvent(new CardTakenToHandEvent(IsEnemy: true, IsFromCommonDeck: RandomExtensions.GetRandomBool()));
        }
    }

    // Events
    public record CardTakenToHandEvent(
        bool IsEnemy,
        bool IsFromCommonDeck) : IEvent;

    // Commands
    public record TakeCardToHandCommand(bool isPlayer) : ICommand;
}
