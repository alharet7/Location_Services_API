using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;

namespace WebAPI.Configurations.DbConfigurations
{
    public class LineConfiguration : IEntityTypeConfiguration<Line>
    {
        public void Configure(EntityTypeBuilder<Line> builder)
        {
            // Table name
            builder.ToTable("lines");

            // Primary key
            builder.HasKey(l => l.Id)
                .HasName("lines_pkey");

            // Column configurations
            builder.Property(l => l.Id)
                .HasColumnName("id");

            builder.Property(l => l.FirstNodeId)
                .HasColumnName("first_node_id");

            builder.Property(l => l.SecondNodeId)
                .HasColumnName("second_node_id");

            builder.Property(l => l.UpdateStatus)
                .HasColumnName("update_status");

            builder.Property(l => l.IsTwoWay)
                .HasColumnName("is_two_way");

            // Relationships
            builder.HasOne(l => l.FirstNode)
                .WithMany(n => n.FirstNodeLines)
                .HasForeignKey(l => l.FirstNodeId);

            builder.HasOne(l => l.SecondNode)
                .WithMany(n => n.SecondNodeLines)
                .HasForeignKey(l => l.SecondNodeId);
        }
    }
}
