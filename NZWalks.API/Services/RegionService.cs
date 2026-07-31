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
            region = await regionRepository.CreateRegionAsync(region);
            return mapper.Map<RegionDto>(region);
        }

        public async Task<RegionDto?> DeleteRegionAsync(Guid id)
        {
            var deleted = await regionRepository.DeleteRegionAsync(id);
            return deleted == null ? null : mapper.Map<RegionDto>(deleted);
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
            var updated = await regionRepository.UpdateRegionAsync(id, region);
            return updated == null ? null : mapper.Map<RegionDto>(updated);
        }
    }
}
