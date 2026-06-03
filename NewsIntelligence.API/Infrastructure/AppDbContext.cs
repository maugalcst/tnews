using Microsoft.EntityFrameworkCore;
using NewsIntelligence.API.Domain;

namespace NewsIntelligence.API.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<ScrapingLog> ScrapingLogs { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        
    }
}