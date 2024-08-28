using FooBooLooGameAPI.DTO;
using FooBooLooGameAPI.DTO.Session;

namespace FooBooLooGameAPI.Interfaces;
public interface ISessionService
{
    Task<ResponseDto> StartSessionAsync(StartSessionDto startSessionDto);

    Task<ResponseDto> GetSessionAsync(int sessionId);

    Task<ResponseDto> SubmitAnswerAsync(SubmitAnswerDto submitAnswerDto, int sessionId);

    Task<ResponseDto> GetSessionResultsAsync(int sessionId);
}