using WebAPI.Entities;
using WebAPI.Models.DTOs;

namespace WebAPI.Services.FloorService
{
    public interface IFloorService
    {
        public Task<FloorDto> GetById(int id);
        public Task<List<FloorDto>> GetAllFloors();
        public Task<FloorDto> Create(FloorDto floorDto);
        public Task<FloorDto> Update(FloorDto floorDto);

        public Task Delete(int id);
    }
}
