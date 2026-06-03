namespace NewsIntelligence.API.Features
{
    public record SourceDto(
        Guid Id,
        string Name,
        string Url,
        string Category,
        bool IsActive
    );
}