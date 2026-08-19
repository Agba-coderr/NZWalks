using NZWalks.API.Models.Common;
using NZWalks.API.Models.DTO;
using NZWalks.API.Models.Enums;

namespace NZWalks.API.Services
{
    public interface IWalkService
    {
        Task<Result> GetAllWalksAsync(string? filterOn = null, string? filterQuery = null);

        Task<Result> GetWalksByUserIdAsync(string userId);

        Task<Result> GetWalksByRegionIdAsync(Guid regionId);

        Task<Result> GetWalksByDifficultyAsync(DifficultyType difficulty);

        Task<Result> GetLongestWalkByUserIdAsync(string userId);

        Task<Result> GetWalkByIdAsync(Guid id);

        Task<Result> CreateWalkAsync(AddWalkRequestDto addWalkRequestDto, string createdByUserId);

        Task<Result> UpdateWalkAsync(Guid id, UpdateWalkDto updateWalkDto, string currentUserId, bool isAdmin);

        Task<Result> DeleteWalkAsync(Guid id, string currentUserId, bool isAdmin);
    }
}
