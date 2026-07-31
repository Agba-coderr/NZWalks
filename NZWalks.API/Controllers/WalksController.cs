using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using NZWalks.API.Services;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkService walkService;

        public WalksController(IWalkService walkService)
        {
            this.walkService = walkService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalks([FromQuery] string? filterOn, [FromQuery] string? filterQuery)
        {
            var walksDto = await walkService.GetAllWalksAsync(filterOn, filterQuery);
            return Ok(walksDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
        {
            var walkDto = await walkService.GetWalkByIdAsync(id);

            if (walkDto == null)
            {
                return NotFound();
            }

            return Ok(walkDto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            var walkDto = await walkService.CreateWalkAsync(addWalkRequestDto);

            return CreatedAtAction(nameof(GetWalkById), new { id = walkDto.Id }, walkDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid id, [FromBody] UpdateWalkDto updateWalkDto)
        {
            var walkDto = await walkService.UpdateWalkAsync(id, updateWalkDto);

            if (walkDto == null)
            {
                return NotFound();
            }

            return Ok(walkDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteWalk(Guid id)
        {
            var walkDto = await walkService.DeleteWalkAsync(id);

            if (walkDto == null)
            {
                return NotFound();
            }

            return Ok(walkDto);
        }
    }
}
