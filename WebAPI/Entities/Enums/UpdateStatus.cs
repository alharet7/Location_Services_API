using NpgsqlTypes;
using System.Runtime.Serialization;

namespace WebAPI.Entities.Enums
{
    public enum UpdateStatus
    {
        [PgName("New")]
        [EnumMember(Value = "New")]
        New,

        [PgName("Updated")]
        [EnumMember(Value = "Updated")]
        Updated,

        [PgName("Deleted")]
        [EnumMember(Value = "Deleted")]
        Deleted
    }
}
