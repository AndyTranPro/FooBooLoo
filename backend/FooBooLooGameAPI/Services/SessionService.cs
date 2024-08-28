using FooBooLooGameAPI.DTO;
using FooBooLooGameAPI.DTO.Session;
using FooBooLooGameAPI.Entities;
using FooBooLooGameAPI.Helpers;
using FooBooLooGameAPI.Interfaces;
using FooBooLooGameAPI.Mappers;
using System.Text;

namespace FooBooLooGameAPI.Services;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IGameRepository _gameRepository;

    public SessionService(ISessionRepository sessionRepository, IGameRepository gameRepository)
    {
        _sessionRepository = sessionRepository;
        _gameRepository = gameRepository;
    }

    public async Task<ResponseDto> StartSessionAsync(StartSessionDto startSessionDto)
    {
        var game = await _gameRepository.GetGameByIdAsync(startSessionDto.GameId);

        if (game == null)
        {
            throw new ArgumentException("Invalid GameId, the game does not exist.");
        }

        var response = startSessionDto.ToCreateSessionDto(game);

        var session = await _sessionRepository.CreateSessionAsync(response);
        if (session == null) return ResponseHandler.errorResponse("Failed to create session");

        return ResponseHandler.successResponse(session, "Session created successfully", 201);
    }

    public async Task<ResponseDto> GetSessionAsync(int sessionId)
    {
        var session = await _sessionRepository.GetSessionByIdAsync(sessionId);

        if (session == null)
        {
            return ResponseHandler.errorResponse("Invalid SessionId, the session does not exist.", 404);
        }

        return ResponseHandler.successResponse(session, "Session retrieved successfully", 200);
    }

    private async Task<ResponseDto> GetNextRandomNumberAsync(int sessionId)
    {
        var session = await _sessionRepository.GetSessionByIdAsync(sessionId);

        if (session == null)
        {
            return ResponseHandler.errorResponse("Invalid SessionId, the session does not exist.");
        }

        int min = session.Game.Min;
        int max = session.Game.Max;

        int nextRandomNumber;
        var random = new Random();

        do
        {
            nextRandomNumber = random.Next(min, max + 1); // Generate random number between min and max (inclusive)
        }
        while (await _sessionRepository.IsNumberExistedAsync(sessionId, nextRandomNumber));

        var sessionNumber = SessionMappers.ToCreateSessionNumber(sessionId, nextRandomNumber, false);

        await _sessionRepository.AddSessionNumberAsync(sessionNumber);

        return ResponseHandler.successResponse(nextRandomNumber, "Number generated successfully", 200);
    }

    public async Task<ResponseDto> SubmitAnswerAsync(SubmitAnswerDto submitAnswerDto, int sessionId)
    {
        var session = await _sessionRepository.GetSessionByIdAsync(sessionId);

        if (session == null)
        {
            return ResponseHandler.errorResponse("Invalid SessionId, the session does not exist.", 404);
        }

        // Assuming StartTime is of type DateTime and Duration is in minutes
        DateTime sessionEndTime = session.StartTime.AddSeconds(session.Duration);
        DateTime currentTime = DateTime.UtcNow;

        // Check if the session time has expired
        if (currentTime >= sessionEndTime)
        {
            await _sessionRepository.EndSessionAsync(sessionId);
            return ResponseHandler.successResponse(session, "Session time has expired", 200);
        }

        bool isNumberAlreadyGuessed = await _sessionRepository.IsNumberServedAsync(sessionId, submitAnswerDto.Number);

        if (isNumberAlreadyGuessed)
        {
            return ResponseHandler.errorResponse("The number is already guessed", 400);
        }
        // Generate expected answer based on the game rules
        string expectedAnswer = GenerateExpectedAnswer(session.Game.RuleSet, submitAnswerDto.Number);
        // Check if the submitted answer is correct
        bool isCorrect = string.Equals(expectedAnswer.Trim(), submitAnswerDto.Answer.Trim(), StringComparison.Ordinal);
        var sessionNumber = session.SessionNumbers.FirstOrDefault(sn => sn.NumberServed == submitAnswerDto.Number);
        // Update the session score
        sessionNumber.IsCorrect = isCorrect;
        sessionNumber.IsPending = false;
        if (isCorrect) session.Score++;
        // Add the submitted number to the session numbers and generate the next random number
        var nextNumber = await GetNextRandomNumberAsync(sessionId);
        await _sessionRepository.SaveChangesAsync();
        return ResponseHandler.successResponse(nextNumber.data, "Answer submitted successfully, serving another number", 200);
    }

    private static string GenerateExpectedAnswer(List<GameRule> rules, int number)
    {
        var result = new StringBuilder();

        foreach (var rule in rules)
        {
            if (number % rule.Divisor == 0)
            {
                result.Append(rule.Replacement);
            }
        }

        return result.Length > 0 ? result.ToString() : number.ToString();
    }

    public async Task<ResponseDto> GetSessionResultsAsync(int sessionId)
    {
        var sessionResult = await _sessionRepository.GetSessionResultsAsync(sessionId);
        if (sessionResult == null) return ResponseHandler.errorResponse("Not found session or is not ended yet.", 404);

        return ResponseHandler.successResponse(sessionResult, "Session results retrieved successfully", 200);
    }
}
