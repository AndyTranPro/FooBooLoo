using FooBooLooGameAPI.Data;
using FooBooLooGameAPI.DTO.Session;
using FooBooLooGameAPI.Entities;
using FooBooLooGameAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FooBooLooGameAPI.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly GameDbContext _context;

    public SessionRepository(GameDbContext context)
    {
        _context = context;
    }

    public async Task<Session> CreateSessionAsync(Session session)
    {
        _context.Sessions.Add(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task<Session> GetSessionByIdAsync(int sessionId)
    {
        return await _context.Sessions
            .Include(s => s.Game).ThenInclude(g => g.RuleSet)
            .Include(s => s.SessionNumbers)
            .FirstOrDefaultAsync(s => s.SessionId == sessionId);
    }

    public Task AddSessionNumberAsync(SessionNumber sessionNumber)
    {
        _context.SessionNumbers.Add(sessionNumber);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }


    public async Task<bool> IsNumberServedAsync(int sessionId, int number)
    {
        return await _context.SessionNumbers
            .AnyAsync(sn => sn.SessionId == sessionId && sn.NumberServed == number && !sn.IsPending);
    }

    public async Task<bool> IsNumberExistedAsync(int sessionId, int number)
    {
        return await _context.SessionNumbers
            .AnyAsync(sn => sn.SessionId == sessionId && sn.NumberServed == number);
    }

    public async Task<Session> EndSessionAsync(int sessionId)
    {
        var session = await GetSessionByIdAsync(sessionId);
        if (session == null || session.IsEnded) return null;

        session.IsEnded = true;
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task<SessionResultDto> GetSessionResultsAsync(int sessionId)
    {
        var session = await GetSessionByIdAsync(sessionId);

        session.IsEnded = true;
        await _context.SaveChangesAsync();

        var totalQuestions = await _context.SessionNumbers
            .CountAsync(sn => sn.SessionId == sessionId);

        var correctAnswers = await _context.SessionNumbers
            .CountAsync(sn => sn.SessionId == sessionId && sn.IsCorrect);

        return new SessionResultDto
        {
            SessionId = session.SessionId,
            PlayerName = session.PlayerName,
            TotalQuestions = totalQuestions,
            CorrectAnswers = correctAnswers,
            IncorrectAnswers = totalQuestions - correctAnswers,
            FinalScore = session.Score,
        };
    }
}
