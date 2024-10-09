using Newtonsoft.Json;
using WebAPI.Entities.Enums;
using WebAPI.Interfaces;

namespace WebAPI.Entities
{
    public class Node: IEntity
    {
        public int Id { get; set; }
        public int FloorId { get; set; }
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Long { get; set; }
        public decimal Lat { get; set; }
        public UpdateStatus UpdateStatus { get; set; }
        public NodeType NodeType { get; set; }

        // Navigation properties
        [JsonIgnore]
        public Floor Floor { get; set; }
        public ICollection<Line> FirstNodeLines { get; set; }
        public ICollection<Line> SecondNodeLines { get; set; }
    }

}
