using NZWalks.API.Models.DTO;

namespace NZWalks.API.Services
{
    public interface IWalkService
    {
        Task<List<WalkDto>> GetAllWalksAsync(string? filterOn = null, string? filterQuery = null);

        Task<WalkDto?> GetWalkByIdAsync(Guid id);

        Task<WalkDto> CreateWalkAsync(AddWalkRequestDto addWalkRequestDto);

        Task<WalkDto?> UpdateWalkAsync(Guid id, UpdateWalkDto updateWalkDto);

        Task<WalkDto?> DeleteWalkAsync(Guid id);
    }
}
