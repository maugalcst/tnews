using Microsoft.AspNetCore.Mvc;
using NewsIntelligence.API.Domain;

namespace NewsIntelligence.API.Features.Sources
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

        [HttpGet]
        public async Task<ActionResult> GetSources()
        {
            List<SourceDto> sources = await _sourceService.GetSourcesAsync();
            return Ok(sources);
        }

    }
}