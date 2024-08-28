namespace FooBooLooGameAPI.DTO.Session;

public class SessionResultDto
{
    public int SessionId { get; set; }

    public string PlayerName { get; set; } = string.Empty;

    public int TotalQuestions { get; set; }

    public int CorrectAnswers { get; set; }

    public int IncorrectAnswers { get; set; }

    public int FinalScore { get; set; }
}

