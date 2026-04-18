using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Endpoints;
using GameStore.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

var connString = "Data Source=GameStore.db";

builder.Services.AddSqlite<GameStoreContext>(
    connString,
    optionsAction: options => options.UseSeeding((context, _) =>
    {
        if(!context.Set<Genre>().Any())
        {
            context.Set<Genre>().AddRange(
                new Genre { Name = "Fighting" },
                new Genre { Name = "RPG" },
                new Genre { Name = "Platformer" },
                new Genre { Name = "Racing" },
                new Genre { Name = "Sports" }
            );

            context.SaveChanges();
        }
    })
);

var app = builder.Build();

app.MapGamesEndpoints();

app.MigrateDb();

app.Run();
