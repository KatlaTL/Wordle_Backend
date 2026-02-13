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
        var presentButWrongPosition = new List<int>();
        var solutionLetters = solution.ToCharArray();
        var guessLetters = guess.Value.ToCharArray();
        var remainingLetters = new Dictionary<char, int>();

        char emptyChar = '\0';

        for (int i = 0; i < solutionLetters.Length; i++)
        {
            if (solutionLetters[i] == guessLetters[i])
            {
                correctPositions.Add(i);
                solutionLetters[i] = emptyChar;
            }
            else
            {
                remainingLetters[guessLetters[i]] = remainingLetters.GetValueOrDefault(guessLetters[i]) + 1;
            }
        }

        for (int i = 0; i < solutionLetters.Length; i++)
        {
            if (solutionLetters[i] != emptyChar && solutionLetters.Contains(guessLetters[i]) && remainingLetters.TryGetValue(guessLetters[i], out int count) && count > 0)
            {
                presentButWrongPosition.Add(i);
                remainingLetters.Remove(guessLetters[i]);
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
            PresentButWrongPositions = presentButWrongPosition.ToArray()
        });
    }

}