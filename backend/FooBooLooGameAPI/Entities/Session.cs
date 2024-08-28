namespace FooBooLooGameAPI.Entities;

public class Session
{
    public int SessionId { get; set; }

    public int GameId { get; set; }

    public string PlayerName { get; set; } = string.Empty;

    public int Duration { get; set; }

    public int Score { get; set; }

    public bool IsEnded { get; set; } = false;

    public Game Game { get; set; } = null!;

    //Storing the numbers served in the session
    public List<SessionNumber> SessionNumbers { get; set; }

    public DateTime StartTime { get; set; } = DateTime.UtcNow;
}
