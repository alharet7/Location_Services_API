using WebAPI.Entities;
using WebAPI.Entities.Enums;

namespace WebAPI.Models.DTOs
{
    public class VenueDto
    {
        public int Id { get; set; }
        public string Name { get; set; }


        // Navigation property
        public ICollection<Floor> Floors { get; set; }
    }
}
