using FooBooLooGameAPI.DTO.Session;
using FooBooLooGameAPI.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FooBooLooGameAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowSpecificOrigin")]
public class SessionsController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionsController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost]
    public async Task<IActionResult> StartSession([FromBody] StartSessionDto dto)
    {
        var response = await _sessionService.StartSessionAsync(dto);

        return StatusCode(response.statusCode, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSession(int id)
    {
        var session = await _sessionService.GetSessionAsync(id);

        return Ok(session);
    }

    [HttpPost("{sessionId}/submit-answer")]
    public async Task<IActionResult> SubmitAnswer(int sessionId, [FromBody] SubmitAnswerDto dto)
    {
        var response = await _sessionService.SubmitAnswerAsync(dto, sessionId);

        return StatusCode(response.statusCode, response);
    }

    [HttpGet("{sessionId}/results")]
    public async Task<IActionResult> GetSessionResults(int sessionId)
    {
        var response = await _sessionService.GetSessionResultsAsync(sessionId);

        return StatusCode(response.statusCode, response);
    }
}

