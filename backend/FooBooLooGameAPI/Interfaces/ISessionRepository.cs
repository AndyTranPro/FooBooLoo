using FooBooLooGameAPI.DTO.Session;
using FooBooLooGameAPI.Entities;

namespace FooBooLooGameAPI.Interfaces;

public interface ISessionRepository
{
    Task<Session> CreateSessionAsync(Session session);
    Task<Session> GetSessionByIdAsync(int sessionId);
    Task<bool> IsNumberServedAsync(int sessionId, int number);
    Task<bool> IsNumberExistedAsync(int sessionId, int number);
    Task AddSessionNumberAsync(SessionNumber sessionNumber);
    Task SaveChangesAsync();
    Task<Session> EndSessionAsync(int sessionId);
    Task<SessionResultDto> GetSessionResultsAsync(int sessionId);
}
