using WebAPI.Entities;
using WebAPI.Entities.Enums;

namespace WebAPI.Models.DTOs
{
    public class FloorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int VenueId { get; set; }
        public int Level { get; set; }


        // Navigation properties
        public VenueDto Venue { get; set; }
        public List<Node> Nodes { get; set; }
    }
}
