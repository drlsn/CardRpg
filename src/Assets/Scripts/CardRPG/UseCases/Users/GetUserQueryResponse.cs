namespace CardRPG.UseCases.Users
{
    public record GetUserQueryResponse(
        string Id,
        uint Version,
        string LastGameId,
        int TutorialStep);
}
