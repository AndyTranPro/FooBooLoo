using FooBooLooGameAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FooBooLooGameAPI.DTO.Game;

[ModelBinder(BinderType = typeof(GameRuleDtoModelBinder))]
public class GameRuleDto
{
    public int Divisor { get; set; }
    public string Replacement { get; set; }
}