using WebAPI.Entities;
using WebAPI.Entities.Enums;

namespace WebAPI.Models.DTOs
{
    public class LineDto
    {
        public int Id { get; set; }
        public int FirstNodeId { get; set; }
        public int SecondNodeId { get; set; }
        public bool IsTwoWay { get; set; }


        //  NPs
        public Node FirstNode { get; set; }
        public Node SecondNode { get; set; }
    }
}
