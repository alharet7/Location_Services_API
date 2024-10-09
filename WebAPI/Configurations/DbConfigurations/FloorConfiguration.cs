using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;

namespace WebAPI.Configurations.DbConfigurations
{
    public class FloorConfiguration : IEntityTypeConfiguration<Floor>
    {
        public void Configure(EntityTypeBuilder<Floor> builder)
        {
            // Table name
            builder.ToTable("floors");

            // Primary key
            builder.HasKey(f => f.Id)
                .HasName("floors_pkey"); 

            // Column configurations
            builder.Property(f => f.Id)
                .HasColumnName("id"); 

            builder.Property(f => f.Name)
                .IsRequired()
                .HasColumnName("name");

            builder.Property(f => f.VenueId)
                .HasColumnName("venue_id");

            builder.Property(f => f.Level)
                .HasColumnName("level"); 

            builder.Property(f => f.UpdateStatus)
                .HasColumnName("update_status");

            // Relationships
            builder.HasOne(f => f.Venue)
                .WithMany(v => v.Floors)
                .HasForeignKey(f => f.VenueId);

            // Navigation property
            builder.HasMany(f => f.Nodes)
                .WithOne(n => n.Floor)
                .HasForeignKey(n => n.FloorId);
        }
    }
}
