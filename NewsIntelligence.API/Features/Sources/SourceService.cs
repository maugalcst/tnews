using Microsoft.EntityFrameworkCore;
using NewsIntelligence.API.Domain;
using NewsIntelligence.API.Features;
using NewsIntelligence.API.Infrastructure;

namespace NewsIntelligence.API.Features.Sources
{
    public class SourceService
    {
        private readonly AppDbContext _context;

        public SourceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateSourceAsync(CreateSourceDto dto)
        {
            var source = new Source(
                name: dto.Name,
                url: dto.Url,
                category: dto.Category,
                xPathTitle: dto.XPathTitle,
                xPathContent: dto.XPathContent
            );

            _context.Sources.Add(source);
            await _context.SaveChangesAsync();
            
            return source.Id;
        }

        public async Task<List<SourceDto>> GetSourcesAsync()
        {
            return await _context.Sources
                .Select(source => new SourceDto(
                    source.Id,
                    source.Name,
                    source.Url,
                    source.Category,
                    source.IsActive
                ))
                .ToListAsync();
        }
    }
}