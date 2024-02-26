using CardRPG.UI.UseCases;
using Core.Net.Http;

namespace CardRPG.UseCases
{
    public record GetGameStateQuery(
        [FromRoute] string GameId) : IQuery<GetGameStateQueryOut>;

    public record GetGameStateQueryOut(
        string Id, uint Version,
        GameStateDTO State,
        PlayerDTO Player,
        PlayerDTO[] Enemies,
        bool HasCommonCards,
        CardDTO? CenterCard = null,
        string? CenterCardPlayerId = null) : IQueryResponse;

    public record GameStateDTO(
        string[] ExpectedActionTypes,
        string[] ExpectedPlayers = null,
        string[] AlreadyMadeActionsPlayers = null,
        bool MustObeyOrder = false);

    public record PlayerDTO(
        string PlayerId,
        CardDTO Hero,
        CardDeckDTO BattlingDeck,
        CardDeckDTO HandDeck,
        bool HasIdleCards,
        DiceOutcomeDTO[]? DiceOutcomes = null,
        CardAssignmentDTO[]? CardAssignments = null);

    public record CardDeckDTO(
        CardDTO[] Cards);

    public record CardDTO(
        string Id,
        bool IsReversed,
        string Name = "",
        string Race = "",
        string Class = "",
        string Fraction = "",
        string Description = "",
        string Type = "",
        CardStatisticsDTO? Statistics = null);

    public record CardStatisticsDTO(
        CardStatisticDTO Attack,
        CardStatisticDTO HP,
        CardStatisticDTO Speed,
        CardStatisticDTO Power);

    public record CardStatisticDTO(
        int Original,
        int Current);

    public record DiceOutcomeDTO(string Value);

    public record CardAssignmentDTO(
        string DiceOutcome,
        int DiceOutcomeIndex = -1,
        int? SkillIndex = -1,
        string SourceCardId = null,
        string[] TargetCardIds = null);
}