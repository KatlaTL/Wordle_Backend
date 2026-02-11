using Moq;
using Xunit;

namespace WordleApi.Tests;

public class GameServiceTests
{
    private readonly GameService _service;
    private readonly Mock<IGameRepository> _gameRepoMock = new();
    private readonly Mock<IWordRepository> _wordRepoMock = new();
    public GameServiceTests()
    {
        _service = new GameService(_wordRepoMock.Object, _gameRepoMock.Object);
    }

    [Fact]
    public void NewGame_ShouldReturnStartGameResponseDto()
    {
        Word solution = new("table");
        _wordRepoMock.Setup(x => x.GetAll()).Returns(new List<Word> { solution });
        _gameRepoMock.Setup(x => x.CreateGame(solution)).Returns(new Game { Solution = solution });

        var result = _service.NewGame();

        Assert.IsType<StartGameResponseDto>(result);
        Assert.Equal(5, result.WordLength);
        Assert.Equal(6, result.AttemptsLeft);
    }

    [Fact]
    public void GetGame_ShouldFail_WhenGameIsNotFound()
    {
        var gameId = Guid.NewGuid();

        _gameRepoMock.Setup(x => x.GetGame(gameId)).Returns((Game?)null); //Cast null to type Game?

        var result = _service.GetGame(gameId);

        Assert.False(result.IsSuccess);
        Assert.Equal("Game not found", result.Error);
    }

    [Theory]
    [InlineData("")] // empty string
    [InlineData("abc")] // too short
    [InlineData("abcdef")] // too long
    public void EvaluateGuess_ShouldFail_WhenWordLengthIsNotEqual5(string word)
    {
        var gameId = Guid.NewGuid();

        var result = _service.EvaluateGuess(gameId, word);

        Assert.False(result.IsSuccess);
        Assert.Equal("Word must be exactly 5 letters", result.Error);
    }

    [Fact]
    public void EvaluateGuess_ShouldReturnCorrectPositions()
    {
        var gameId = Guid.NewGuid();

        _gameRepoMock.Setup(x => x.GetGame(gameId)).Returns(new Game { Solution = new("table") });

        var result = _service.EvaluateGuess(gameId, "taste");

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Contains(0, result.Value.CorrectPositions);
        Assert.Contains(4, result.Value.CorrectPositions);
    }

    [Fact]
    public void EvaluateGuess_ShouldReturnPresentButWrongPosition()
    {
        var gameId = Guid.NewGuid();

        _gameRepoMock.Setup(x => x.GetGame(gameId)).Returns(new Game { Solution = new("table") });

        var result = _service.EvaluateGuess(gameId, "glass");

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Contains(1, result.Value.PresentButWrongPosition);
        Assert.Contains(2, result.Value.PresentButWrongPosition);
    }

    [Fact]
    public void EvaluateGuess_ShouldHandleDuplicateLettersCorrectly()
    {
        var gameId = Guid.NewGuid();

        _gameRepoMock.Setup(x => x.GetGame(gameId)).Returns(new Game { Solution = new("apple") });

        var result = _service.EvaluateGuess(gameId, "ppppp");

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.CorrectPositions.Length + result.Value.PresentButWrongPosition.Length);
    }

}
