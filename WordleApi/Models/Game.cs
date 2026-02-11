public class Game
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Word Solution { get; set; } = null!;
    public int AttemptsLeft { get; set; } = 6;
    public bool IsFinished { get; set; } = false;
}