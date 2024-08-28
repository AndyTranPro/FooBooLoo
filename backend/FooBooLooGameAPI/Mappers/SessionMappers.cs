using FooBooLooGameAPI.DTO.Session;
using FooBooLooGameAPI.Entities;

namespace FooBooLooGameAPI.Mappers;

public static class SessionMappers
{
    public static Session ToCreateSessionDto(this StartSessionDto startSessionDto, Game game)
    {
        var random = new Random();
        int firstNumber = random.Next(game.Min, game.Max + 1);

        return new Session
        {
            GameId = startSessionDto.GameId,
            PlayerName = startSessionDto.PlayerName,
            Duration = startSessionDto.Duration,
            SessionNumbers = new List<SessionNumber>
            {
                new SessionNumber
                {
                    NumberServed = firstNumber,
                    IsCorrect = false,
                    IsPending = true
                }
            }
        };
    }


    public static SessionNextNumberDto ToSessionNextNumberDto(this SessionNumber sessionNumber)
    {
        return new SessionNextNumberDto
        {
            NextNumberServed = sessionNumber.NumberServed
        };
    }

    public static SessionNumber ToCreateSessionNumber(int sessionId, int numberServed, bool isCorrect)
    {
        return new SessionNumber
        {
            SessionId = sessionId,
            NumberServed = numberServed,
            IsCorrect = isCorrect,
            IsPending = true
        };
    }
}