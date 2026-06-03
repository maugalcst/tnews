namespace NewsIntelligente.API.Features.Sources
{
    public record CreateSourceDto(
        string Name,
        string Url,
        string Category
    );
}