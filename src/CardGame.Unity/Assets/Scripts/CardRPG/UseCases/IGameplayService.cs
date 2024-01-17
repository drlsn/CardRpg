using System;

namespace CardRPG.UI.UseCases
{
    public interface IGameplayService
    {
        void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent;
        void Send<TCommand>(TCommand command) where TCommand : ICommand;
    }

    public interface IEvent
    {

    }

    public interface ICommand
    {

    }
}
