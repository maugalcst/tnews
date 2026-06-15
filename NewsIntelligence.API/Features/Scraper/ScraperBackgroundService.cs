using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace NewsIntelligence.API.Features.Scraper
{
    public class ScraperBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ScraperBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("[BACKGROUND WORKER] The background worker is background-working °u° ...");
                
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var scraper = scope.ServiceProvider.GetRequiredService<ScraperService>();
                        await scraper.ScrapeAllSourcesAsync();
                    }
                }
                catch (Exception e)
                {
                    // Absorbemos el golpe. Imprimimos el error, pero NO hacemos throw.
                    Console.WriteLine($"[ERROR BACKGROUND WORKER] Failed: {e.Message}");
                }

                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }
                
    }
}