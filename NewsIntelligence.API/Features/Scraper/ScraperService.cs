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
            return await _context.Sources.Where(source => source.IsActive.Equals(true)).ToListAsync();
        }

        public async Task ScrapeAllSourcesAsync()
        {
            var sources = await GetActiveSourcesAsync();

            var web = new HtmlWeb();

            foreach (var source in sources)
            {
                try
                {
                    System.Console.WriteLine($"[SCRAPER] Initializing download of: {source.Name} ({source.Url})...");
                    HtmlDocument doc = await web.LoadFromWebAsync(source.Url);

                    var titleNode = doc.DocumentNode.SelectSingleNode(source.XPathTItle);
                    var contentNode = doc.DocumentNode.SelectSingleNode(source.XPathContent);

                    if (titleNode == null || contentNode == null)
                    {
                        throw new Exception($"The XPATH selectors didn't match with the current {source.Name} HTML");
                    }

                    string articleTitle = titleNode.InnerText.Trim();
                    string articleContent = contentNode.InnerText.Trim();

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
                    System.Console.WriteLine($"[SCRAPER] Success! Extracted article: \"{articleTitle}\"");

                } catch (Exception e)
                {
                    System.Console.WriteLine($"[ERROR SCRAPER] The processing of {source.Name} failed: {e.Message}");
                    continue;
                }
            }  

            await _context.SaveChangesAsync();
            System.Console.WriteLine($"[SCRAPER] Massive execution finalized.");
        }

        public async Task<List<ArticleDto>> GetScrapedArticlesAsync()
        {
            return await _context.Articles
                .Select(article => new ArticleDto(
                    article.Id,
                    article.Title,
                    article.Content,
                    article.ScrapedDate
                ))
                .ToListAsync();
        }

    }    
}