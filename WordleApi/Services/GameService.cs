public class GameService : IGameService
{
    private readonly IWordRepository _wordRepository;
    private readonly IGameRepository _gameRepository;
    private static readonly Random _rand = Random.Shared;

    public GameService(IWordRepository wordRepository, IGameRepository gameRepository)
    {
        _wordRepository = wordRepository;
        _gameRepository = gameRepository;
    }
    public StartGameResponseDto NewGame()
    {
        var words = _wordRepository.GetAll();
        var solution = words[_rand.Next(words.Count)];
        Console.WriteLine(solution);
        return _gameRepository.CreateGame(solution).toStartGameResponseDto();
    }

    public Result<StartGameResponseDto> GetGame(Guid gameId)
    {
        var game = _gameRepository.GetGame(gameId);

        if (game == null) return Result<StartGameResponseDto>.Failure("Game not found");

        return Result<StartGameResponseDto>.Success(game.toStartGameResponseDto());
    }

    public Result<GuessWordResponseDto> EvaluateGuess(Guid gameId, string word)
    {
        if (String.IsNullOrWhiteSpace(word) || word.Length != 5) return Result<GuessWordResponseDto>.Failure("Word must be exactly 5 letters");

        var game = _gameRepository.GetGame(gameId);

        if (game == null) return Result<GuessWordResponseDto>.Failure("Game not found");

        var solution = game.Solution.Value;
        var guess = new Word(word);

        var correctPositions = new List<int>();
        var presentButWrongPosition = new Dictionary<char, int>();

        for (int i = 0; i < solution.Length; i++)
        {
            if (solution[i] == guess.Value[i])
            {
                correctPositions.Add(i);

            } else if (!presentButWrongPosition.ContainsKey(guess.Value[i]) && solution.Contains(guess.Value[i])) {
                presentButWrongPosition.Add(guess.Value[i], i);
            }
        }

        game.AttemptsLeft--;

        bool isCorrect = string.Equals(game.Solution.Value, guess.Value, StringComparison.OrdinalIgnoreCase);

        if (isCorrect || game.AttemptsLeft <= 0)
        {
            game.IsFinished = true;
            _gameRepository.EndGame(gameId);
        }

        return Result<GuessWordResponseDto>.Success(new GuessWordResponseDto
        {
            Word = guess.Value,
            AttemptsLeft = game.AttemptsLeft,
            IsCorrect = isCorrect,
            IsFinished = game.IsFinished,
            CorrectPositions = correctPositions.ToArray(),
            PresentButWrongPosition = presentButWrongPosition.Values.ToArray()
        });
    }

}