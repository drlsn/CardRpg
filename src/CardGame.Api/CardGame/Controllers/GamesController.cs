using CardGame.Entities;
using CardGame.UseCases;
using CardGame.Utilities;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace CardGame.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesController : ControllerBase
{
    private readonly ICommandHandler<PlayNextCardCommand, Result> _playNextCardCommandHandler;
    private readonly ICommandHandler<CreateGameCommand, Result> _createGameCommandHandler;
    private readonly ICommandHandler<JoinGameCommand, Result> _joinGameCommandHandler;

    public GamesController(
        ICommandHandler<PlayNextCardCommand, Result> playNextCardCommandHandler, 
        ICommandHandler<CreateGameCommand, Result> createGameCommandHandler, 
        ICommandHandler<JoinGameCommand, Result> joinGameCommandHandler)
    {
        _playNextCardCommandHandler = playNextCardCommandHandler;
        _createGameCommandHandler = createGameCommandHandler;
        _joinGameCommandHandler = joinGameCommandHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateGameCommand command)
    {
        var result = await _createGameCommandHandler.Handle(command, CancellationToken.None);
        return result.ToActionResult();
    }

    [HttpPost("playNextCard")]
    public async Task<IActionResult> Get(PlayNextCardCommand command)
    {
        var result = await _playNextCardCommandHandler.Handle(command, CancellationToken.None);
        return result.ToActionResult();
    }

    [HttpPost("joinGame")]
    public async Task<IActionResult> Get(JoinGameCommand command)
    {
        var result = await _joinGameCommandHandler.Handle(command, CancellationToken.None);
        return result.ToActionResult();
    }
}
