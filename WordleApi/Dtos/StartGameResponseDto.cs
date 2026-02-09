public class StartGameResponseDto
{
    public Guid GameId { get; set; }

    public string Word { get; set;} = String.Empty;

    public int AttemptsLeft {get; set;}
}