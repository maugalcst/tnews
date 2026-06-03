using NewsIntelligente.API.Domain;
using NewsIntelligente.API.Infrastructure;

namespace NewsIntelligence.API.Features.Sources
{
    public class SourceService
    {
        private readonly AppDbContext _context;

        public SourceService(AppDbContext context)
        {
            _context = context;
        }
    }
}