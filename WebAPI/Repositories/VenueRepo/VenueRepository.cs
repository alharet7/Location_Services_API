using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;
using WebAPI.Entities.Enums;
using WebAPI.Models;

namespace WebAPI.Repositories.VenueRepo
{
    public class VenueRepository : IVenueRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public VenueRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task Create(Venue venue)
        {
            venue.UpdateStatus = UpdateStatus.New;
           _dbContext.Add(venue);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Venue> GetById(int id)
        {
            return await _dbContext.Venues
                                               .Include(venue => venue.Floors)
                                                    .ThenInclude(floor => floor.Nodes)
                                                         .ThenInclude(node => node.FirstNodeLines)
                                               .Include(venue => venue.Floors)
                                                  .ThenInclude(floor => floor.Nodes)
                                                  .ThenInclude(node => node.SecondNodeLines)
                                               .FirstOrDefaultAsync(venue => venue.Id == id && 
                                                                              venue.UpdateStatus != UpdateStatus.Deleted);
        }

        public IQueryable<Venue> GetVenues()
        {
            return _dbContext.Venues
                                       .AsNoTracking()
                                       .Include(venue => venue.Floors)
                                              .ThenInclude(floor => floor.Nodes)
                                                    .ThenInclude(node => node.FirstNodeLines)
                                       .Include(venue => venue.Floors)
                                              .ThenInclude(floor => floor.Nodes)
                                                   .ThenInclude(node => node.SecondNodeLines)
                                       .Where(venue => venue.UpdateStatus != UpdateStatus.Deleted);

        }

        public async Task Update(Venue venue)
        {
            venue.UpdateStatus = UpdateStatus.Updated;
            _dbContext.Update(venue);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var venue = await _dbContext.Venues.FirstOrDefaultAsync(v => v.Id == id);
           
                venue.UpdateStatus = UpdateStatus.Deleted;
                _dbContext.Venues.Update(venue);
                await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsExist(int id)
        {
            return await _dbContext.Venues
                                               .AnyAsync(v => v.Id == id && v.UpdateStatus != UpdateStatus.Deleted);
        }
    }
}
