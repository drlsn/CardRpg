using CardRPG.UI.UseCases;
using System;
using System.Threading.Tasks;

namespace CardRPG.Infrastructure
{
    internal class OnlineGameplayService : IGameplayService
    {
        Task<TQueryResponse> IGameplayService.Query<TQuery, TQueryResponse>()
        {
            throw new NotImplementedException();
        }

        Task<TQueryResponse> IGameplayService.Query<TQuery, TQueryResponse>(TQuery query)
        {
            throw new NotImplementedException();
        }

        void IGameplayService.Send<TCommand>(TCommand command)
        {
            throw new NotImplementedException();
        }

        void IGameplayService.Subscribe<TEvent>(Action<TEvent> handler)
        {
            throw new NotImplementedException();
        }
    }
}
