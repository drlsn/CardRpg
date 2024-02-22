using System;
using System.Threading.Tasks;

namespace CardRPG.UI.UseCases
{
    public interface IGameplayService
    {
        void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent;
        void Send<TCommand>(TCommand command) where TCommand : ICommand;
        Task<TQueryResponse> Query<TQuery, TQueryResponse>()
            where TQueryResponse : class, IQueryResponse
            where TQuery : class, IQuery<TQueryResponse>, new();

        Task<TQueryResponse> Query<TQuery, TQueryResponse>(TQuery query)
            where TQueryResponse : class, IQueryResponse
            where TQuery : class, IQuery<TQueryResponse>;
    }

    public interface IGameEventsDispatcher
    {
        void Dispatch<TEvent>(IEvent @event);
    }

    public interface IEvent {}
    public interface ICommand {}
    public interface IQuery<TResponse> where TResponse : IQueryResponse {}
    public interface IQueryResponse {}
}
