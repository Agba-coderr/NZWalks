using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.DTO;
using NZWalks.API.Models.Enums;
using NZWalks.API.Services;
using System.Security.Claims;

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
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAllWalks([FromQuery] string? filterOn, [FromQuery] string? filterQuery)
        {
            var walksDto = await walkService.GetAllWalksAsync(filterOn, filterQuery);
            return Ok(walksDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
        {
            var walkDto = await walkService.GetWalkByIdAsync(id);

            if (walkDto == null)
            {
                return NotFound();
            }

            return Ok(walkDto);
        }

        [HttpGet]
        [Route("user")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetWalksByUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var walksDto = await walkService.GetWalksByUserIdAsync(userId);

            return Ok(walksDto);
        }

        [HttpGet]
        [Route("user/longest")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetLongestWalkByUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var longestWalkDto = await walkService.GetLongestWalkByUserId(userId);

            if (longestWalkDto == null)
            {
                return NotFound();
            }

            return Ok(longestWalkDto);
        }

        [HttpGet]
        [Route("region/{regionId:Guid}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetWalksByRegionId([FromRoute] Guid regionId)
        {
            var walksDto = await walkService.GetWalksByRegionIdAsync(regionId);

            if (walksDto == null)
            {
                return NotFound();
            }

            return Ok(walksDto);
        }

        [HttpGet]
        [Route("difficulty")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetWalksByDifficulty(DifficultyType difficulty)
        {
            var walksDto = await walkService.GetWalksByDifficultyAsync(difficulty);

            if (walksDto == null)
            {
                return NotFound();
            }

            return Ok(walksDto);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            // get the logged-in user id from token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }
            
            var walkDto = await walkService.CreateWalkAsync(addWalkRequestDto, userId);

            return CreatedAtAction(nameof(GetWalkById), new { id = walkDto.Id }, walkDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid id, [FromBody] UpdateWalkDto updateWalkDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var isAdmin = User.IsInRole("Admin");

            var walkDto = await walkService.UpdateWalkAsync(id, updateWalkDto, userId, isAdmin);

            if (walkDto == null)
            {
                return NotFound();
            }

            return Ok(walkDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteWalk(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var isAdmin = User.IsInRole("Admin");

            var walkDto = await walkService.DeleteWalkAsync(id, userId, isAdmin);

            if (walkDto == null)
            {
                return NotFound();
            }

            return Ok(walkDto);
        }
    }
}