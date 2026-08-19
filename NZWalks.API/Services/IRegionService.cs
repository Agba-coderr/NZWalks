using NZWalks.API.Models.Common;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Services
{
    public interface IRegionService
    {
        Task<Result> GetAllRegionsAsync();

        Task<Result> GetRegionByIdAsync(Guid id);

        Task<Result> CreateRegionAsync(AddRegionRequestDto addRegionRequestDto);

        Task<Result> UpdateRegionAsync(Guid id, UpdateRegionDto updateRegionDto);

        Task<Result> DeleteRegionAsync(Guid id);
    }
}

