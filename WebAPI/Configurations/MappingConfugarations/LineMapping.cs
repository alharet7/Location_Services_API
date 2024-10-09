using Mapster;
using WebAPI.Entities;
using WebAPI.Models.DTOs;

namespace WebAPI.Configurations.MappingConfugarations
{
    public class LineMapping
    {
        public static void ConfigureMappings()
        {
            TypeAdapterConfig<Line, LineDto>.NewConfig();
        }
    }
}
