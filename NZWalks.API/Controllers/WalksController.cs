using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Common;
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
        [Authorize(Roles = "Reader,Writer,Admin")]
        public async Task<IActionResult> GetAllWalks([FromQuery] string? filterOn, [FromQuery] string? filterQuery)
        {
            
            var result = await walkService.GetAllWalksAsync(filterOn, filterQuery);

            return StatusCode(result.Status, result);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader,Writer,Admin")]
        public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
        {
            var result = await walkService.GetWalkByIdAsync(id);

            return StatusCode(result.Status, result);
        }

        [HttpGet]
        [Route("user")]
        [Authorize(Roles = "Writer,Admin")]
        public async Task<IActionResult> GetWalksByUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                var failureResponse = Result.Failure("User is not authenticated", 401);
                
                return StatusCode(failureResponse.Status, failureResponse);
            }

            var result = await walkService.GetWalksByUserIdAsync(userId);

            return StatusCode(result.Status, result);
        }

        [HttpGet]
        [Route("user/longest")]
        [Authorize(Roles = "Writer,Admin")]
        public async Task<IActionResult> GetLongestWalkByUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                var failureResponse = Result.Failure("User is not authenticated", 401);

                return StatusCode(failureResponse.Status, failureResponse);
            }

            var result = await walkService.GetLongestWalkByUserIdAsync(userId);

            return StatusCode(result.Status, result);
        }

        [HttpGet]
        [Route("region/{regionId:Guid}")]
        [Authorize(Roles = "Reader,Writer,Admin")]
        public async Task<IActionResult> GetWalksByRegionId([FromRoute] Guid regionId)
        {
            var result = await walkService.GetWalksByRegionIdAsync(regionId);

            return StatusCode(result.Status, result);
        }

        [HttpGet]
        [Route("difficulty")]
        [Authorize(Roles = "Reader,Writer,Admin")]
        public async Task<IActionResult> GetWalksByDifficulty(DifficultyType difficulty)
        {
            var result = await walkService.GetWalksByDifficultyAsync(difficulty);

            return StatusCode(result.Status, result);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer,Admin")]
        public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                var failureResponse = Result.Failure("User is not authenticated", 401);

                return StatusCode(failureResponse.Status, failureResponse);
            }
            
            var result = await walkService.CreateWalkAsync(addWalkRequestDto, userId);

            return StatusCode(result.Status, result);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer,Admin")]
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid id, [FromBody] UpdateWalkDto updateWalkDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                var failureResponse = Result.Failure("User is not authenticated", 401);

                return StatusCode(failureResponse.Status, failureResponse);
            }

            var isAdmin = User.IsInRole("Admin");

            var result = await walkService.UpdateWalkAsync(id, updateWalkDto, userId, isAdmin);

            return StatusCode(result.Status, result);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer,Admin")]
        public async Task<IActionResult> DeleteWalk(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                var failureResponse = Result.Failure("User is not authenticated", 401);

                return StatusCode(failureResponse.Status, failureResponse);
            }

            var isAdmin = User.IsInRole("Admin");

            var result = await walkService.DeleteWalkAsync(id, userId, isAdmin);

            return StatusCode(result.Status, result);
        }
    }
}