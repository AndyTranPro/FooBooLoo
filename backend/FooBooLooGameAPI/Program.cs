using FooBooLooGameAPI.Data;
using FooBooLooGameAPI.Extensions;
using FooBooLooGameAPI.Interfaces;
using FooBooLooGameAPI.Repositories;
using FooBooLooGameAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
//builder.WebHost.UseUrls("http://*:80");

// Create a single instance of NpgsqlDataSourceBuilder
var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
dataSourceBuilder.EnableDynamicJson(); // Enable dynamic JSON serialization

// Build the data source once and reuse it
var dataSource = dataSourceBuilder.Build();

// Add services to the container.
builder.Services.AddDbContext<GameDbContext>(options =>
{
    options.UseNpgsql(dataSource);
});

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<ISessionService, SessionService>();

// Add authorization
builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder => policyBuilder.WithOrigins("http://localhost:5173", "http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My Game API",
        Version = "v1"
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();
// Use the CORS policy
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();

app.MapControllers();

app.Run();