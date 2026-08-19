using AutoMapper;
using NZWalks.API.Models.Common;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Services
{
    public class RegionService : IRegionService
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionService(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        public async Task<Result> CreateRegionAsync(AddRegionRequestDto addRegionRequestDto)
        {
            var region = mapper.Map<Region>(addRegionRequestDto);
            var createdRegion = await regionRepository.CreateRegionAsync(region);

            return Result.Success(mapper.Map<RegionDto>(createdRegion), "Region created successfully");
        }

        public async Task<Result> DeleteRegionAsync(Guid id)
        {
            var deletedRegion = await regionRepository.DeleteRegionAsync(id);

            if (deletedRegion == null)
            {
                return Result.Failure($"Region with ID {id} was not found", 404);
            }

            return Result.Success(mapper.Map<RegionDto>(deletedRegion), "Region deleted successfully");
        }

        public async Task<Result> GetAllRegionsAsync()
        {
            var regions = await regionRepository.GetAllRegionsAsync();

            return Result.Success(mapper.Map<List<RegionDto>>(regions), "Regions retrieved successfully");
        }

        public async Task<Result> GetRegionByIdAsync(Guid id)
        {
            var region = await regionRepository.GetRegionByIdAsync(id);

            if (region == null)
            {
                return Result.Failure($"Region with ID {id} was not found", 404);
            }

            return Result.Success(mapper.Map<RegionDto>(region), "Region retrieved successfully");
        }

        public async Task<Result> UpdateRegionAsync(Guid id, UpdateRegionDto updateRegionDto)
        {
            var region = mapper.Map<Region>(updateRegionDto);
            var updatedRegion = await regionRepository.UpdateRegionAsync(id, region);

            if (updatedRegion == null)
            {
                return Result.Failure($"Region with ID {id} was not found", 404);
            }

            return Result.Success(mapper.Map<RegionDto>(updatedRegion), "Region updated successfully");
        }
    }
}

