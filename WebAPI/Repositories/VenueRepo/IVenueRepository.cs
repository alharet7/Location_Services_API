using WebAPI.Entities;

namespace WebAPI.Repositories.VenueRepo
{
    public interface IVenueRepository
    {
        public IQueryable<Venue> GetVenues();
        public Task<Venue> GetById(int id);
        public Task Update(Venue venue);
        public Task Create(Venue venue);
        public Task Delete(int id);
        public Task<bool> IsExist(int id);
    }
}
