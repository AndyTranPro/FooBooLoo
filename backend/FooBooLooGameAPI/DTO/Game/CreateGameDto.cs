namespace FooBooLooGameAPI.DTO.Game;

public class CreateGameDto
{
    public string Name { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Min { get; set; }
    public int Max { get; set; }
    public List<GameRuleDto> RuleSet { get; set; }
}
