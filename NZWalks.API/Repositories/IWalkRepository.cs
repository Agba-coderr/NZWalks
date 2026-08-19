using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Models.Enums;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<List<Walk>> GetAllWalksAsync(string? filterOn = null, string? filterQuery = null);

        Task<List<Walk>> GetWalksByUserIdAsync(string userId);

        Task<List<Walk>> GetWalksByRegionIdAsync(Guid regionId);

        Task<List<Walk>> GetWalksByDifficultyAsync(DifficultyType difficulty);

        Task<Walk?> GetLongestWalkByUserIdAsync(string userId);

        Task<Walk?> GetWalkByIdAsync(Guid id);

        Task<Walk> CreateWalkAsync(Walk walk);

        Task<Walk?> UpdateWalkAsync(Guid id, Walk walk);

        Task<Walk?> DeleteWalkAsync(Guid id);
    }
}
