
public interface IGameService
{
   StartGameResponseDto NewGame();
   Result<StartGameResponseDto> GetGame(Guid gameId);
   Result<GuessWordResponseDto> EvaluateGuess(Guid gameId, string word);
}