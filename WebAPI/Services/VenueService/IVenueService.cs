using WebAPI.Entities;
using WebAPI.Models.DTOs;

namespace WebAPI.Services.VenueService
{
    public interface IVenueService
    {
        public Task<VenueDto> GetById(int id);
        public Task<List<VenueDto>> GetAllVenues();
        public Task<VenueDto> Create(VenueDto venueDto);
        public Task<VenueDto> Update(VenueDto venueDto);
        public Task Delete(int venueId);
    }
}
