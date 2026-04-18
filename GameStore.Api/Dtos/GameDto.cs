namespace GameStore.Api.Dtos;

public record GameDto(
    int Id,
    string Name,
    string Gender,
    decimal Price,
    DateOnly ReleaseDate
);
