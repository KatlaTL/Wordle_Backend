using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/wordle")]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;
    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpPost("game")]
    public ActionResult<StartGameResponseDto> NewGame()
    {
        var dto = _gameService.NewGame();
        return CreatedAtAction(nameof(GetGame), new { gameId = dto.GameId }, dto);
    }

    [HttpGet("game/{gameId}")]
    public ActionResult<StartGameResponseDto> GetGame(Guid gameId)
    {
        var result = _gameService.GetGame(gameId);

        if (!result.IsSuccess) return NotFound();

        return Ok(result.Value);
    }

    [HttpPost("game/{gameId}/guess")]
    public ActionResult<GuessWordResponseDto> Guess(Guid gameId, [FromBody] GuessWordRequestDto request)
    {
        var result = _gameService.EvaluateGuess(gameId, request.Word);

        if (!result.IsSuccess) return BadRequest(result.Error);

        return Ok(result.Value);
    }
}