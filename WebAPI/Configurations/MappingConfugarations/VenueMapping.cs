using Mapster;
using WebAPI.Entities;
using WebAPI.Models.DTOs;

namespace WebAPI.Configurations.MappingConfugarations
{
    public class VenueMapping
    {
        public static void ConfigureMappings()
        {
            TypeAdapterConfig<Venue, VenueDto>.NewConfig()
            .Map(dest => dest.Floors, src => src.Floors.Adapt<List<FloorDto>>());
        }
    }
}
