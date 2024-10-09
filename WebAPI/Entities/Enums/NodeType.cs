using NpgsqlTypes;

namespace WebAPI.Entities.Enums
{
    public enum NodeType
    {
        [PgName("Elevator")]
        Elevator,
        [PgName("Escalator")]
        Escalator,
        [PgName("Ramp")]
        Ramp,
        [PgName("Stairs")]
        Stairs,
        [PgName("Normal")]
        Normal
    }

}
