using CardRPG.UI.UseCases;

namespace CardRPG.UseCases.Games
{
    public record StartGameCommand(bool Random = true, bool Bot = true) : ICommand;
}
