using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Services
{
    public class WalkService : IWalkService
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalkService(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        public async Task<WalkDto> CreateWalkAsync(AddWalkRequestDto addWalkRequestDto)
        {
            var walk = mapper.Map<Walk>(addWalkRequestDto);
            walk = await walkRepository.CreateWalkAsync(walk);
            return mapper.Map<WalkDto>(walk);
        }

        public async Task<WalkDto?> DeleteWalkAsync(Guid id)
        {
            var deleted = await walkRepository.DeleteWalkAsync(id);
            return deleted == null ? null : mapper.Map<WalkDto>(deleted);
        }

        public async Task<List<WalkDto>> GetAllWalksAsync(string? filterOn = null, string? filterQuery = null)
        {
            var walks = await walkRepository.GetAllWalksAsync(filterOn, filterQuery);
            return mapper.Map<List<WalkDto>>(walks);
        }

        public async Task<WalkDto?> GetWalkByIdAsync(Guid id)
        {
            var walk = await walkRepository.GetWalkByIdAsync(id);
            return walk == null ? null : mapper.Map<WalkDto>(walk);
        }

        public async Task<WalkDto?> UpdateWalkAsync(Guid id, UpdateWalkDto updateWalkDto)
        {
            var walk = mapper.Map<Walk>(updateWalkDto);
            var updated = await walkRepository.UpdateWalkAsync(id, walk);
            return updated == null ? null : mapper.Map<WalkDto>(updated);
        }
    }
}
