
using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;
using WebAPI.Entities.Enums;

namespace WebAPI.Repositories.FloorRepo
{
    public class FloorRepository : IFloorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public FloorRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Floor> GetById(int id)
        {
            return await _dbContext.Floors
                                               .Include(floor => floor.Nodes)
                                                  .ThenInclude(node => node.FirstNodeLines)
                                               .Include(floor => floor.Nodes)
                                                  .ThenInclude(node => node.SecondNodeLines)
                                               .FirstOrDefaultAsync(floor => floor.Id == id &&
                                                                              floor.UpdateStatus != UpdateStatus.Deleted);
        }


        public IQueryable<Floor> GetFloors()
        {
            return _dbContext.Floors
                     .AsNoTracking()
                     .Include(floor => floor.Nodes)
                         .ThenInclude(node => node.FirstNodeLines)
                     .Include(floor => floor.Nodes)
                         .ThenInclude(node => node.SecondNodeLines)
                     .Where(floor => floor.UpdateStatus != UpdateStatus.Deleted);
        }



        public async Task Update(Floor floor)
        {
            floor.UpdateStatus = UpdateStatus.Updated;
            _dbContext.Update(floor);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Create(Floor floor)
        {
            floor.UpdateStatus = UpdateStatus.New;
            _dbContext.Add(floor);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var floor = await GetById(id);

            floor.UpdateStatus = UpdateStatus.Deleted;
            _dbContext.Floors.Update(floor);
            await _dbContext.SaveChangesAsync();
        }


        public IQueryable<Floor> GetFloorsByVenueId(int venueId)
        {
            return _dbContext.Floors.Where(f => f.VenueId == venueId &&
                                                           f.UpdateStatus != UpdateStatus.Deleted);
        }
        public async Task<bool> IsExist(int id)
        {
            return await _dbContext.Floors
                                               .AnyAsync(f => f.Id == id && f.UpdateStatus != UpdateStatus.Deleted);
        }

    }
}
