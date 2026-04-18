using System;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string EndpointName = "GetName";

    private static readonly List<GameDto> games = [
        new (
            1, 
            "Street Figther II", 
            "Fighting", 
            19.99M, 
            new DateOnly(1992, 7, 15)
            ),
        new (
            2, 
            "Final Fantasy VII Rebirth", 
            "RPG", 
            69.99M, 
            new DateOnly(2024, 2, 29)
            ),
        new (
            3, 
            "Astro Bot", 
            "Platformer", 
            59.99M, 
            new DateOnly(2024, 9, 6)
            )
    ];

    public static void MapGamesEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("/games");

        group.MapGet("/", () => games);

        group.MapGet("/{id}", (int id) => {

            var game = games.Find(f => f.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
            
        }).WithName(EndpointName);

        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            if(string.IsNullOrEmpty(newGame.Name))
            {
                return Results.BadRequest("Name is required");
            }

            Game game = new()
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            GameDetailsDto gameDto = new(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(EndpointName, new {id = game.Id}, gameDto);
        });

        group.MapPut("/{id}", (int id, UpdateGameDto updateGame) =>
        {
            var index = games.FindIndex(f => f.Id == id);

            if(index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDto(
                id,
                updateGame.Name,
                updateGame.Genre,
                updateGame.Price,
                updateGame.ReleaseDate
            );

            return Results.NoContent();
        });

        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(f => f.Id == id);  

            return Results.NoContent(); 
        });

    }
}
