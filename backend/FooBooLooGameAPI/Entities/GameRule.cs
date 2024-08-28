using FooBooLooGameAPI.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace FooBooLooGameAPI.Entities;

[ModelBinder(BinderType = typeof(GameRuleDtoModelBinder))]
public class GameRule
{
    public int Id { get; set; }

    public int GameId { get; set; }

    public int Divisor { get; set; }

    public string Replacement { get; set; } = string.Empty;

    [ForeignKey("GameId")]
    public Game Game { get; set; }
}