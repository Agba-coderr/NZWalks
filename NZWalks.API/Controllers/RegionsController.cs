using NZWalks.API.Services;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionService regionService;

        public RegionsController(IRegionService regionService)
        {
            this.regionService = regionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regionsDto = await regionService.GetAllRegionsAsync();
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            var regionDto = await regionService.GetRegionByIdAsync(id);

            if (regionDto == null)
            {
                return NotFound();
            }

            return Ok(regionDto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            var regionDto = await regionService.CreateRegionAsync(addRegionRequestDto);

            return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionDto updateRegionDto)
        {
            var regionDto = await regionService.UpdateRegionAsync(id, updateRegionDto);

            if (regionDto == null)
            {
                return NotFound();
            }

            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            var regionDto = await regionService.DeleteRegionAsync(id);

            if (regionDto == null)
            {
                return NotFound();
            }

            return Ok(regionDto);

        }

    }
}
