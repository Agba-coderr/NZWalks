using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
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

            var response = Result.Success(walksDto, "Walks retrieved successfully");

            return StatusCode(response.Status, response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
        {
            var walkDto = await walkService.GetWalkByIdAsync(id);

            if (walkDto == null)
            {
                var failureResponse = Result.Failure($"Walk with ID {id} was not found", 404);

                return StatusCode(failureResponse.Status, failureResponse);
            }

            var successResponse = Result.Success(walkDto, "Walk retrieved successfully");

            return StatusCode(successResponse.Status, successResponse);
        }

        [HttpGet]
        [Route("user")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetWalksByUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                var failureResponse = Result.Failure("User is not authenticated", 401);
                
                return StatusCode(failureResponse.Status, failureResponse);
            }

            var walksDto = await walkService.GetWalksByUserIdAsync(userId);

            var successResponse = Result.Success(walksDto, "Walks retrieved successfully");

            return StatusCode(successResponse.Status, successResponse);
        }

        [HttpGet]
        [Route("user/longest")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetLongestWalkByUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                var failureResponse = Result.Failure("User is not authenticated", 401);

                return StatusCode(failureResponse.Status, failureResponse);
            }

            var longestWalkDto = await walkService.GetLongestWalkByUserId(userId);

            if (longestWalkDto == null)
            {
                var failureResponse = Result.Failure($"Longest walk for user {userId} was not found", 404);

                return StatusCode(failureResponse.Status, failureResponse);
            }

            var successResponse = Result.Success(longestWalkDto, "Longest walk retrieved successfully");

            return StatusCode(successResponse.Status, successResponse);
        }

        [HttpGet]
        [Route("region/{regionId:Guid}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetWalksByRegionId([FromRoute] Guid regionId)
        {
            var walksDto = await walkService.GetWalksByRegionIdAsync(regionId);

            if (walksDto == null)
            {
                var failureResponse = Result.Failure($"Walks for region {regionId} were not found", 404);

                return StatusCode(failureResponse.Status, failureResponse);
            }

            var successResponse = Result.Success(walksDto, "Walks retrieved successfully");

            return StatusCode(successResponse.Status, successResponse);
        }

        [HttpGet]
        [Route("difficulty")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetWalksByDifficulty(DifficultyType difficulty)
        {
            var walksDto = await walkService.GetWalksByDifficultyAsync(difficulty);

            if (walksDto == null)
            {
                var failureResponse = Result.Failure($"Walks with difficulty:{difficulty} were not found", 404);

                return StatusCode(failureResponse.Status, failureResponse);
            }

            var successResponse = Result.Success(walksDto, $"Walks with difficulty:{difficulty} retrieved successfully");

            return StatusCode(successResponse.Status, successResponse);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                var failureResponse = Result.Failure("User is not authenticated", 401);

                return StatusCode(failureResponse.Status, failureResponse);
            }
            
            var walkDto = await walkService.CreateWalkAsync(addWalkRequestDto, userId);

            var successResponse = Result.Success(walkDto, "Walk created successfully", 201);

            return StatusCode(successResponse.Status, successResponse);
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
                var failureResponse = Result.Failure("User is not authenticated", 401);

                return StatusCode(failureResponse.Status, failureResponse);
            }

            var isAdmin = User.IsInRole("Admin");

            var walkDto = await walkService.UpdateWalkAsync(id, updateWalkDto, userId, isAdmin);

            if (walkDto == null)
            {
                var failureResponse = Result.Failure("Walk not found", 404);

                return StatusCode(failureResponse.Status, failureResponse);
            }

            var successResponse = Result.Success(walkDto, "Walk updated successfully");

            return StatusCode(successResponse.Status, successResponse);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteWalk(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                var failureResponse = Result.Failure("User is not authenticated", 401);

                return StatusCode(failureResponse.Status, failureResponse);
            }

            var isAdmin = User.IsInRole("Admin");

            var walkDto = await walkService.DeleteWalkAsync(id, userId, isAdmin);

            if (walkDto == null)
            {
                var failureResponse = Result.Failure("Walk not found", 404);

                return StatusCode(failureResponse.Status, failureResponse);
            }

            var successResponse = Result.Success(walkDto, "Walk deleted successfully");

            return StatusCode(successResponse.Status, successResponse);
        }
    }
}