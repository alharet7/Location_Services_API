using Mapster;
using WebAPI.Entities;
using WebAPI.Models.DTOs;

namespace WebAPI.Configurations.MappingConfugarations
{
    public class NodeMapping
    {
        public static void ConfigureMappings()
        {
            TypeAdapterConfig<Node, NodeDto>.NewConfig()
            .Map(dest => dest.FirstNodeLines, src => src.FirstNodeLines.Adapt<List<LineDto>>())
            .Map(dest => dest.SecondNodeLines, src => src.SecondNodeLines.Adapt<List<LineDto>>());
        }
    }
}
