public interface IGameRepository
{
    Game CreateGame(Word solution);
    Game? GetGame(Guid gameId);
    void EndGame(Guid gameId);
}