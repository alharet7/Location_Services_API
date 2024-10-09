using Newtonsoft.Json;
using WebAPI.Entities.Enums;
using WebAPI.Interfaces;

namespace WebAPI.Entities
{
    public class Line: IEntity
    {
        public int Id { get; set; }
        public int FirstNodeId { get; set; }
        public int SecondNodeId { get; set; }
        public UpdateStatus UpdateStatus { get; set; }
        public bool IsTwoWay { get; set; }

        // Navigation properties
        [JsonIgnore]
        public Node FirstNode { get; set; }
        [JsonIgnore]
        public Node SecondNode { get; set; }
    }

}
