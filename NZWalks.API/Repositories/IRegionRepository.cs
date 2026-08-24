using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<(List<Region> Regions, int TotalCount)> GetAllRegionsAsync(int pageNumber = 1, int pageSize = 10);

        Task<Region?> GetRegionByIdAsync(Guid id);

        Task<Region> CreateRegionAsync(Region region);

        Task<Region?> UpdateRegionAsync(Guid id, Region region);

        Task<Region?> DeleteRegionAsync(Guid id);
    }
}
