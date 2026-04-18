namespace GameStore.Api.Dtos;

public record UpdateGameDto(
    string Name,
    string Gender,
    decimal Price,
    DateOnly ReleaseDate
);