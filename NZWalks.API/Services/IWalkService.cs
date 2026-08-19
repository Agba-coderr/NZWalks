using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Models.Enums;

namespace NZWalks.API.Services
{
    public interface IWalkService
    {
        Task<List<WalkDto>> GetAllWalksAsync(string? filterOn = null, string? filterQuery = null);

        Task<List<WalkDto>> GetWalksByUserIdAsync(string userId);

        Task<List<WalkDto>> GetWalksByRegionIdAsync(Guid regionId);

        Task<List<WalkDto>> GetWalksByDifficultyAsync(DifficultyType difficulty);

        Task<WalkDto?> GetLongestWalkByUserId(string userId);

        Task<WalkDto?> GetWalkByIdAsync(Guid id);

        Task<Result> CreateWalkAsync(AddWalkRequestDto addWalkRequestDto, string createdByUserId);

        Task<Result> UpdateWalkAsync(Guid id, UpdateWalkDto updateWalkDto, string currentUserId, bool isAdmin);

        Task<Result> DeleteWalkAsync(Guid id, string currentUserId, bool isAdmin);
    }
}
