using System.Diagnostics;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using NewsIntelligence.API.Domain;
using NewsIntelligence.API.Infrastructure;

namespace NewsIntelligence.API.Features.Scraper
{
    public class ScraperService
    {
        private readonly AppDbContext _context;

        public ScraperService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Source>> GetActiveSourcesAsync()
        {
            return await _context.Sources.Where(source => source.IsActive).ToListAsync();
        }

        public async Task ScrapeAllSourcesAsync()
        {
            var sources = await GetActiveSourcesAsync();
            var web = new HtmlWeb();

            foreach (var source in sources)
            {
                string status = "Failed";
                int articlesCount = 0;
                Stopwatch duration = Stopwatch.StartNew();

                try
                {
                    System.Console.WriteLine($"[SCRAPER] Initializing download of: {source.Name} ({source.Url})...");
                    HtmlDocument doc = await web.LoadFromWebAsync(source.Url);

                    var titleNodes = doc.DocumentNode.SelectNodes(source.XPathTitle);
                    var contentNodes = doc.DocumentNode.SelectNodes(source.XPathContent);

                    if (titleNodes == null || contentNodes == null)
                    {
                        throw new Exception($"The XPATH selectors didn't match with the current {source.Name} HTML");
                    }

                    articlesCount = Math.Min(titleNodes.Count, contentNodes.Count);

                    for (int i = 0; i < articlesCount; i++)
                    {
                        string articleTitle = titleNodes[i].InnerText.Trim();
                        string articleContent = contentNodes[i].InnerText.Trim();

                        var article = new Article(
                            title: articleTitle,
                            author: "Scraper Bot",
                            content: articleContent,
                            url: source.Url,
                            category: source.Category,
                            publishedDate: DateTimeOffset.UtcNow,
                            sourceId: source.Id
                        );

                        _context.Articles.Add(article);
                    }

                    System.Console.WriteLine($"[SCRAPER] Success! Extracted {articlesCount} articles from {source.Name}!");

                    status = "Success";

                } 
                catch (Exception e)
                {
                    System.Console.WriteLine($"[ERROR SCRAPER] The processing of {source.Name} failed: {e.Message}");
                    status = $"Failed: {e.Message}";
                }
                finally
                {
                    duration.Stop();
                    long durationMs = duration.ElapsedMilliseconds;

                    ScrapingLog lastScrap = new ScrapingLog(status, durationMs, articlesCount, source.Id);
                    _context.ScrapingLogs.Add(lastScrap);

                    await _context.SaveChangesAsync();
                }
            }

            await _context.SaveChangesAsync();
            System.Console.WriteLine($"[SCRAPER] Massive execution finalized.");
        }

        public async Task<List<ArticleDto>> GetScrapedArticlesAsync()
        {
            return await _context.Articles.AsNoTracking()
                .Select(article => new ArticleDto(
                    article.Id,
                    article.Title,
                    article.Content,
                    article.ScrapedDate
                ))
                .ToListAsync();
        }

        public async Task<List<ScrapingLog>> GetScraperLogsAsync()
        {
            return await _context.ScrapingLogs.AsNoTracking().ToListAsync();
        }

    }    
}