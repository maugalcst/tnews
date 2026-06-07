namespace NewsIntelligence.API.Features.Scraper
{
    public record ArticleDto(
        Guid Id,
        string Title,
        string Content,
        string Url,
        DateTimeOffset ScrapedDate
    );
}