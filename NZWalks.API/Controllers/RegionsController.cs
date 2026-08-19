using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Common;
using NZWalks.API.Models.DTO;
using NZWalks.API.Services;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionService regionService;

        public RegionsController(IRegionService regionService)
        {
            this.regionService = regionService;
        }

        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAllRegions()
        {
            var results = await regionService.GetAllRegionsAsync();

            return StatusCode(results.Status, results);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            var result = await regionService.GetRegionByIdAsync(id);

            return StatusCode(result.Status, result);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            var result = await regionService.CreateRegionAsync(addRegionRequestDto);

            return StatusCode(result.Status, result);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionDto updateRegionDto)
        {
            var result = await regionService.UpdateRegionAsync(id, updateRegionDto);

            return StatusCode(result.Status, result);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            var result = await regionService.DeleteRegionAsync(id);

            return StatusCode(result.Status, result);
        }
    }
}

