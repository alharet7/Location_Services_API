using System.Xml.Linq;
using System;
using WebAPI.Entities.Enums;
using WebAPI.Interfaces;
using Newtonsoft.Json;

namespace WebAPI.Entities
{
    public class Floor : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int VenueId { get; set; }
        public int Level { get; set; }
        
        public UpdateStatus UpdateStatus { get; set; }
        // Navigation properties
        [JsonIgnore]
        public Venue Venue { get; set; }
        public ICollection<Node> Nodes { get; set; }
    }

}
