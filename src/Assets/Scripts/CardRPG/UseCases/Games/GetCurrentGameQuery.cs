using CardRPG.UI.UseCases;

namespace CardRPG.UseCases.Games
{
    public record GetCurrentGameQuery : IQuery<GetCurrentGameQueryOut>;
    public record GetCurrentGameQueryOut(string GameId) : IQueryResponse;
}
