using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Entities;

namespace WebAPI.Configurations.DbConfigurations
{
    public class VenueConfiguration : IEntityTypeConfiguration<Venue>
    {
        public void Configure(EntityTypeBuilder<Venue> builder)
        {
            // Table name
            builder.ToTable("venues");

            // Primary key
            builder.HasKey(v => v.Id)
                .HasName("venues_pkey"); // Specify the PK name from your PostgreSQL database

            // Column configurations
            builder.Property(v => v.Id)
                .HasColumnName("id");

            builder.Property(v => v.Name)
                .IsRequired()
                .HasColumnName("name");

            builder.Property(v => v.UpdateStatus)
                .HasColumnName("update_status");

            // Relationships
            builder.HasMany(v => v.Floors)
                .WithOne(f => f.Venue)
                .HasForeignKey(f => f.VenueId);
        }
    }
}
