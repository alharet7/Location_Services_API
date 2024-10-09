using WebAPI.Entities.Enums;

namespace WebAPI.Interfaces
{
    public interface IEntity
    {
       public int Id { get; set; }
       public UpdateStatus UpdateStatus { get; set; }
    }
}
