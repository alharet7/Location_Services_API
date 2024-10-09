using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;
using WebAPI.Entities.Enums;

namespace WebAPI.Configurations.DbConfigurations
{
    public class NodeConfiguration : IEntityTypeConfiguration<Node>
    {
        public void Configure(EntityTypeBuilder<Node> builder)
        {
            // Table name
            builder.ToTable("nodes");

            // Primary key
            builder.HasKey(n => n.Id)
                .HasName("nodes_pkey");

            // Column configurations
            builder.Property(n => n.Id)
                .HasColumnName("id");

            builder.Property(n => n.FloorId)
                .HasColumnName("floor_id");

            builder.Property(n => n.X)
                .HasColumnName("x");

            builder.Property(n => n.Y)
                .HasColumnName("y");

            builder.Property(n => n.Long)
                .HasColumnName("long");

            builder.Property(n => n.Lat)
                .HasColumnName("lat");

            builder.Property(n => n.UpdateStatus)
                .HasColumnName("update_status");


            builder.Property(e => e.NodeType)
           .HasColumnName("node_type");


            // Relationships
            builder.HasOne(n => n.Floor)
                .WithMany(f => f.Nodes)
                .HasForeignKey(n => n.FloorId);

            // Many-to-many relationships with lines (assuming FirstNodeLines and SecondNodeLines are navigation properties)
            builder.HasMany(n => n.FirstNodeLines)
                .WithOne(l => l.FirstNode)
                .HasForeignKey(l => l.FirstNodeId);

            builder.HasMany(n => n.SecondNodeLines)
                .WithOne(l => l.SecondNode)
                .HasForeignKey(l => l.SecondNodeId);
        }
    }
}
