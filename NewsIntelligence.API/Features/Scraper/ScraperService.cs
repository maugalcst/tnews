using System.Diagnostics;
using System.Text;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using NewsIntelligence.API.Domain;
using NewsIntelligence.API.Features.AI;
using NewsIntelligence.API.Infrastructure;

namespace NewsIntelligence.API.Features.Scraper
{
    public class ScraperService
    {
        private readonly AppDbContext _context;
        private readonly AIService _aiService;

        public ScraperService(AppDbContext context, AIService aiService)
        {
            _context = context;
            _aiService = aiService;
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

                    var articlesCards = doc.DocumentNode.SelectNodes(source.XPathContainer);

                    if (articlesCards == null)
                        throw new Exception($"Couldn't find cards with the specific container for {source.Name}.");

                    int successfulArticlesInThisSource = 0;

                    foreach (var card in articlesCards)
                    {
                        try
                        {
                            string articleTitle = "Unknown";
                            
                            string articleUrl = "Unknown";

                            var titleLinkNode = card.SelectSingleNode(source.XPathTitle);

                            if (titleLinkNode == null) continue;

                            articleTitle = titleLinkNode.InnerText.Trim();

                            articleUrl = titleLinkNode.GetAttributeValue("href", string.Empty);

                            if (string.IsNullOrEmpty(articleUrl)) continue;

                            if (!Uri.TryCreate(articleUrl, UriKind.Absolute, out _))
                            {
                                articleUrl = new Uri(new Uri(source.Url), articleUrl).ToString();
                            }

                            bool articleUrlExist = await _context.Articles.AnyAsync(a => a.Url == articleUrl);

                            if (articleUrlExist) continue;

                            string articleTextRaw = await GetArticleTextAsync(web, articleUrl, source.XPathContent);
                            
                            if (articleTextRaw.Length < 100) continue;

                            string articleContent = await _aiService.GenerateSummaryAsync(articleTextRaw);

                            var article = new Article(
                                title: articleTitle,
                                author: "Scraper bot",
                                content: articleContent,
                                url: articleUrl,
                                category: source.Category,
                                publishedDate: DateTimeOffset.UtcNow,
                                sourceId: source.Id
                            );

                            _context.Add(article);
                            successfulArticlesInThisSource++;

                            System.Console.WriteLine($"[SCRAPER] Processed card: \"{articleTitle}\" -> {articleUrl}");
                        }
                        catch (System.Exception e)
                        {
                            System.Console.WriteLine($"[ERROR SCRAPER] The processing of {card.Name} failed: {e.Message}");
                            continue;
                        }
                        
                    }

                    articlesCount = successfulArticlesInThisSource;
                    
                    if (articlesCards.Count == 0) 
                    {
                        status = "Empty";
                    }
                    else if (successfulArticlesInThisSource == articlesCards.Count) 
                    {
                        status = "Success";
                    }
                    else if (successfulArticlesInThisSource > 0) 
                    {
                        status = "Partial";
                    }
                    else 
                    {
                        status = "Failed";
                    }

                } 
                catch (Exception e)
                {
                    System.Console.WriteLine($"[ERROR SCRAPER] The processing of {source.Name} failed: {e.Message}");
                    continue;
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

            System.Console.WriteLine($"[SCRAPER] Massive execution finalized.");
        }

        public async Task<List<ArticleDto>> GetScrapedArticlesAsync()
        {
            return await _context.Articles.AsNoTracking()
                .Select(article => new ArticleDto(
                    article.Id,
                    article.Title,
                    article.Content,
                    article.Url,
                    article.ScrapedDate
                ))
                .ToListAsync();
        }

        public async Task<List<ScrapingLog>> GetScraperLogsAsync()
        {
            return await _context.ScrapingLogs.AsNoTracking().ToListAsync();
        }

        private async Task<string> GetArticleTextAsync(HtmlWeb web, string articleUrl, string xpathContent)
        {
            try
            {
                HtmlDocument doc = await web.LoadFromWebAsync(articleUrl);
                var contentFound = doc.DocumentNode.SelectNodes(xpathContent);

                if (contentFound == null) return "Content not available for extraction.";
                
                StringBuilder contentBuilder = new StringBuilder();

                foreach (var p in contentFound)
                {
                    var paragraph = p.InnerText.Trim();
                    if (string.IsNullOrEmpty(paragraph)) continue;

                    contentBuilder.AppendLine(paragraph);
                }

                string content = contentBuilder.ToString();
                return content;

            }
            catch (Exception e)
            {
                System.Console.WriteLine($"[WARNING] Error leyendo artículo: {e.Message}");
                return "Error al extraer el contenido completo.";
            }
        }

    }    
}