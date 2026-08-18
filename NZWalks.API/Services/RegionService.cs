using AutoMapper;
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

        public async Task<RegionDto> CreateRegionAsync(AddRegionRequestDto addRegionRequestDto)
        {
            var region = mapper.Map<Region>(addRegionRequestDto);
            var createdRegion = await regionRepository.CreateRegionAsync(region);
            return mapper.Map<RegionDto>(createdRegion);
        }

        public async Task<RegionDto?> DeleteRegionAsync(Guid id)
        {
            var deletedRegion = await regionRepository.DeleteRegionAsync(id);
            return deletedRegion == null ? null : mapper.Map<RegionDto>(deletedRegion);
        }

        public async Task<List<RegionDto>> GetAllRegionsAsync()
        {
            var regions = await regionRepository.GetAllRegionsAsync();
            return mapper.Map<List<RegionDto>>(regions);
        }

        public async Task<RegionDto?> GetRegionByIdAsync(Guid id)
        {
            var region = await regionRepository.GetRegionByIdAsync(id);
            return region == null ? null : mapper.Map<RegionDto>(region);
        }

        public async Task<RegionDto?> UpdateRegionAsync(Guid id, UpdateRegionDto updateRegionDto)
        {
            var region = mapper.Map<Region>(updateRegionDto);
            var updatedRegion = await regionRepository.UpdateRegionAsync(id, region);
            return updatedRegion == null ? null : mapper.Map<RegionDto>(updatedRegion);
        }
    }
}

