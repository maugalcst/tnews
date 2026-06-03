using Microsoft.AspNetCore.Mvc;
using NewsIntelligence.API.Features.Sources;
using NewsIntelligente.API.Domain;

namespace NewsIntelligente.API.Features.Sources
{
    [ApiController]
    [Route("api/sources")]
    public class SourcesController : ControllerBase
    {
        private readonly SourceService _sourceService;

        public SourcesController(SourceService sourceService)
        {
            _sourceService = sourceService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateSource([FromBody] CreateSourceDto dto)
        {
            Guid sourceId = await _sourceService.CreateSourceAsync(dto);
            return Ok(sourceId);
        }

    }
}