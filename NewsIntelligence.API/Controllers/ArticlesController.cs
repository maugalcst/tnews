using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsIntelligence.API.Infrastructure;

namespace NewsIntelligence.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ArticlesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArticlesAsync()
        {
            var articles = await _context.Articles
                .OrderByDescending(a => a.ScrapedDate)
                .Take(20)
                .ToListAsync();
                
            if (!articles.Any())
            {
                return NotFound($"No articles were found in the database.");
            }

            return Ok(articles);
        }
    }
}