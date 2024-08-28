namespace FooBooLooGameAPI.Entities;

public class SessionNumber
{
    public int SessionNumberId { get; set; }

    public int SessionId { get; set; }

    public Session Session { get; set; } = null!;

    public int NumberServed { get; set; }

    public bool IsCorrect { get; set; }

    public bool IsPending { get; set; }
}
