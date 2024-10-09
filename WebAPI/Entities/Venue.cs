using WebAPI.Entities.Enums;
using WebAPI.Interfaces;

namespace WebAPI.Entities
{
    public class Venue: IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UpdateStatus UpdateStatus { get; set; }

        // Navigation property
        public ICollection<Floor> Floors { get; set; }
    }

}
