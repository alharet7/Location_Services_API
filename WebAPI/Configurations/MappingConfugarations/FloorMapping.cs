using Mapster;
using WebAPI.Entities;
using WebAPI.Models.DTOs;

namespace WebAPI.Configurations.MappingConfugarations
{
    public class FloorMapping
    {
        public static void ConfigureMappings()
        {
            TypeAdapterConfig<Floor, FloorDto>.NewConfig()
                .Map(dest => dest.Nodes, src => src.Nodes.Adapt<List<NodeDto>>());
        }
    }
}
