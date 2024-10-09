using WebAPI.Entities.Enums;
using WebAPI.Entities;

namespace WebAPI.Models.DTOs
{
    public class NodeDto
    {
        public int Id { get; set; }
        public int FloorId { get; set; }
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Long { get; set; }
        public decimal Lat { get; set; }
        public string NodeType { get; set; }


        // Navigation properties
        public Floor Floor { get; set; }
        public ICollection<Line> FirstNodeLines { get; set; }
        public ICollection<Line> SecondNodeLines { get; set; }
    }
}
