using System.Net;
using Microsoft.AspNetCore.Mvc;
using NewsIntelligence.API.Infrastructure;

namespace NewsIntelligence.API.Features.Scraper
{
    [ApiController]
    [Route("api/scraper")]
    public class ScraperController : ControllerBase
    {
        private readonly ScraperService _scraperService;

        public ScraperController(ScraperService scraperService)
        {
            _scraperService = scraperService;
        }

        [HttpPost("trigger")]
        public async Task<IActionResult> TriggerScrape()
        {
            await _scraperService.ScrapeAllSourcesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetScrapedArticles()
        {
            var scrapedArticles = await _scraperService.GetScrapedArticlesAsync();
            return Ok(scrapedArticles);
        }

        [HttpGet("logs")]
        public async Task<IActionResult> GetScrapedLogs()
        {
            var scraperLogs = await _scraperService.GetScraperLogsAsync();
            return Ok(scraperLogs);
        }

    }
}