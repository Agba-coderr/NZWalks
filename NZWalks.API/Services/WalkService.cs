using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Models.Enums;
using NZWalks.API.Repositories;

namespace NZWalks.API.Services
{
    public class WalkService : IWalkService
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;
        private readonly IRegionRepository _regionRepository;

        public WalkService(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
            _regionRepository = regionRepository;
        }

        public async Task<Result> CreateWalkAsync(AddWalkRequestDto addWalkRequestDto, string createdByUserId)
        {
            var region = await _regionRepository.GetRegionByIdAsync(addWalkRequestDto.RegionId);

            if (region == null)
            {
                return Result.Failure($"Region ID {addWalkRequestDto.RegionId} does not exist");

            }

            var walk = new Walk
            {
                Name = addWalkRequestDto.Name,
                Description = addWalkRequestDto.Description,
                LengthInKm = addWalkRequestDto.LengthInKm,
                WalkImageUrl = addWalkRequestDto.WalkImageUrl,
                DifficultyType = addWalkRequestDto.DifficultyType,
                RegionId = addWalkRequestDto.RegionId,
                Region = region,
                CreatedByUserId = createdByUserId
            };

            walk = await _walkRepository.CreateWalkAsync(walk);

            return Result.Success(_mapper.Map<WalkDto>(walk), "Walk created successfully");
        }

        public async Task<Result> DeleteWalkAsync(Guid id, string currentUserId, bool isAdmin)
        {
            var existingWalk = await _walkRepository.GetWalkByIdAsync(id);

            if (existingWalk == null)
            {
                return Result.Failure("This walk does not exist.", 404);
            }

            var region = await _regionRepository.GetRegionByIdAsync(existingWalk.RegionId);
            if (region == null)
            {
                return Result.Failure("The region associated with this walk no longer exists.", 400);
            }

            if (!isAdmin && existingWalk.CreatedByUserId != currentUserId)
            {
                return Result.Failure("You do not own this walk.", 403);
            }

            var deleted = await _walkRepository.DeleteWalkAsync(id);

            return Result.Success(_mapper.Map<WalkDto>(deleted), "Walk deleted successfully");
        }

        public async Task<List<WalkDto>> GetAllWalksAsync(string? filterOn = null, string? filterQuery = null)
        {
            var walks = await _walkRepository.GetAllWalksAsync(filterOn, filterQuery);
            return _mapper.Map<List<WalkDto>>(walks);
        }

        public async Task<WalkDto?> GetLongestWalkByUserId(string userId)
        {
            var longestWalk = await _walkRepository.GetLongestWalkByUserId(userId);

            if (longestWalk == null)
            {
                return null;
            }

            return _mapper.Map<WalkDto>(longestWalk);
        }

        public async Task<WalkDto?> GetWalkByIdAsync(Guid id)
        {
            var walk = await _walkRepository.GetWalkByIdAsync(id);
            return walk == null ? null : _mapper.Map<WalkDto>(walk);
        }

        public async Task<List<WalkDto>> GetWalksByDifficultyAsync(DifficultyType difficulty)
        {
            var walks = await _walkRepository.GetWalksByDifficultyAsync(difficulty);

            return _mapper.Map<List<WalkDto>>(walks);
        }

        public async Task<List<WalkDto>> GetWalksByRegionIdAsync(Guid regionId)
        {
            var walks = await _walkRepository.GetWalksByRegionIdAsync(regionId);

            return _mapper.Map<List<WalkDto>>(walks);
        }

        public async Task<List<WalkDto>> GetWalksByUserIdAsync(string userId)
        {
            var walks = await _walkRepository.GetWalksByUserIdAsync(userId);

            return _mapper.Map<List<WalkDto>>(walks);
        }

        public async Task<Result> UpdateWalkAsync(Guid id, UpdateWalkDto updateWalkDto, string currentUserId, bool isAdmin)
        {
            var existingWalk = await _walkRepository.GetWalkByIdAsync(id);

            if (existingWalk == null)
            {
                return Result.Failure("This walk does not exist.", 404);
            }

            if (!isAdmin && existingWalk.CreatedByUserId != currentUserId)
            {
                return Result.Failure("You do not own this walk.", 403);
            }

            var region = await _regionRepository.GetRegionByIdAsync(updateWalkDto.RegionId);
            if (region == null)
            {
                return Result.Failure("Invalid region ID.", 400);
            }

            var walkDomainModel = _mapper.Map<Walk>(updateWalkDto);
            walkDomainModel.Region = region;
            walkDomainModel.CreatedByUserId = existingWalk.CreatedByUserId;

            var updated = await _walkRepository.UpdateWalkAsync(id, walkDomainModel);

            return Result.Success(_mapper.Map<WalkDto>(updated), "Walk updated successfully");
        }
    }
}
