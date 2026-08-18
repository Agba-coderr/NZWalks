using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
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
            var regionsDto = await regionService.GetAllRegionsAsync();
            var response = Result.Success(regionsDto, "Regions retrieved successfully");

            return StatusCode(response.Status, response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            var regionDto = await regionService.GetRegionByIdAsync(id);

            if (regionDto == null)
            {
                var notFoundResponse = Result.Failure($"Region with ID {id} was not found", 404);
                return StatusCode(notFoundResponse.Status, notFoundResponse);
            }

            var response = Result.Success(regionDto, "Region retrieved successfully");
            return StatusCode(response.Status, response);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            var regionDto = await regionService.CreateRegionAsync(addRegionRequestDto);
            var response = Result.Success(regionDto, "Region created successfully", 201);

            return CreatedAtAction(nameof(GetRegionById), new { id = response.Data?.Id }, response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionDto updateRegionDto)
        {
            var regionDto = await regionService.UpdateRegionAsync(id, updateRegionDto);

            if (regionDto == null)
            {
                var notFoundResponse = Result.Failure($"Region with ID {id} was not found", 404);
                return StatusCode(notFoundResponse.Status, notFoundResponse);
            }

            var response = Result.Success(regionDto, "Region updated successfully");
            return StatusCode(response.Status, response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            var regionDto = await regionService.DeleteRegionAsync(id);

            if (regionDto == null)
            {
                var notFoundResponse = Result.Failure($"Region with ID {id} was not found", 404);
                return StatusCode(notFoundResponse.Status, notFoundResponse);
            }

            var response = Result.Success(regionDto, "Region deleted successfully");
            return StatusCode(response.Status, response);
        }
    }
}

