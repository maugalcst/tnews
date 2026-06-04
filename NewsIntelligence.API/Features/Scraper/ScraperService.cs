using Microsoft.AspNetCore.Http.HttpResults;
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

            foreach (var source in sources)
            {
                System.Console.WriteLine($"[Scraper] Visitando: {source.Name} en la URL {source.Url}...");
            }  
        }
    }    
}