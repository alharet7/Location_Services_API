using WebAPI.Entities;

namespace WebAPI.Repositories.FloorRepo
{
    public interface IFloorRepository
    {
        public IQueryable<Floor> GetFloors();
        public Task<Floor> GetById(int id);
        public Task Update(Floor floor);
        public Task Create(Floor floor);
        public Task Delete(int id);
        public IQueryable<Floor> GetFloorsByVenueId(int venueId);
        public Task<bool> IsExist(int id);
    }
}
