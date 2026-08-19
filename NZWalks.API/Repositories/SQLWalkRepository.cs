using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.DTO;
using NZWalks.API.Models.Enums;


namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbcontext;

        public SQLWalkRepository(NZWalksDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public async Task<Walk> CreateWalkAsync(Walk walk)
        {
            await dbcontext.Walks.AddAsync(walk);
            await dbcontext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteWalkAsync(Guid id)
        {
            var existingWalk = await dbcontext.Walks.FindAsync(id);
            if (existingWalk == null)
            {
                return null;
            }

            dbcontext.Walks.Remove(existingWalk);
            await dbcontext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<List<Walk>> GetAllWalksAsync(string? filterOn = null, string? filterQuery = null)
        {
            var walks = dbcontext.Walks.Include(w => w.Region).AsQueryable();
            //Filtering
            if(string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            return await walks.ToListAsync();
        }

        public async Task<Walk?> GetWalkByIdAsync(Guid id)
        {
            return await dbcontext.Walks.Include(w => w.Region).FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<List<Walk>> GetWalksByRegionIdAsync(Guid regionId)
        {
            return await dbcontext.Walks.Include(w => w.Region).Where(w => w.RegionId == regionId).ToListAsync();

        }

        public async Task<List<Walk>> GetWalksByUserIdAsync(string userId)
        {
            return await dbcontext.Walks.Include(w => w.Region).Where(w => w.CreatedByUserId == userId).ToListAsync();
        }

        public async Task<Walk?> GetLongestWalkByUserIdAsync(string userId)
        {
            return await dbcontext.Walks.Include(w => w.Region).Where(w => w.CreatedByUserId == userId).OrderByDescending(w => w.LengthInKm).FirstOrDefaultAsync();
        }

        public async Task<Walk?> UpdateWalkAsync(Guid id, Walk walk)
        {
            var exisitingWalk = await dbcontext.Walks.FindAsync(id);

            if (exisitingWalk == null)
            {
                return null;
            }

            exisitingWalk.Name = walk.Name;
            exisitingWalk.Description = walk.Description;
            exisitingWalk.LengthInKm = walk.LengthInKm;
            exisitingWalk.WalkImageUrl = walk.WalkImageUrl;
            exisitingWalk.DifficultyType = walk.DifficultyType;
            exisitingWalk.RegionId = walk.RegionId;

            await dbcontext.SaveChangesAsync();
            return exisitingWalk;
        }

        public async Task<List<Walk>> GetWalksByDifficultyAsync(DifficultyType difficulty)
        {
            return await dbcontext.Walks.Include(w => w.Region).Where(w => w.DifficultyType == difficulty).ToListAsync();
        }
    }
}
