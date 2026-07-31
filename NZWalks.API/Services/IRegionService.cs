using NZWalks.API.Models.DTO;

namespace NZWalks.API.Services
{
    public interface IRegionService
    {
        Task<List<RegionDto>> GetAllRegionsAsync();

        Task<RegionDto?> GetRegionByIdAsync(Guid id);

        Task<RegionDto> CreateRegionAsync(AddRegionRequestDto addRegionRequestDto);

        Task<RegionDto?> UpdateRegionAsync(Guid id, UpdateRegionDto updateRegionDto);

        Task<RegionDto?> DeleteRegionAsync(Guid id);
    }
}
