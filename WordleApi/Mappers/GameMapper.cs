public static class GameMapper
{
    public static StartGameResponseDto toStartGameResponseDto(this Game game)
    {
        return new StartGameResponseDto
        {
            GameId = game.Id,
            WordLength = game.Solution.Value.Length,
            AttemptsLeft = game.AttemptsLeft
        };
    }
}