using WebAPI.Entities;
using WebAPI.Models.DTOs;

namespace WebAPI.Services.LineService
{
    public interface ILineService
    {
        public Task<LineDto> GetById(int id);
        public Task<List<LineDto>> GetAllLines();
        public Task<LineDto> Create(LineDto lineDto);
        public Task<LineDto> Update(LineDto lineDto);
        public Task Delete(int id);
    }
}
