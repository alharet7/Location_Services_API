using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using WebAPI.Configurations.DbConfigurations;
using WebAPI.Entities.Enums;

namespace WebAPI.Entities
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public ApplicationDbContext()
        {
        }

        static ApplicationDbContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<UpdateStatus>("public.updatestatus")
                                                                 .MapEnum<NodeType>("public.nodetypes");
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public virtual DbSet<Floor> Floors { get; set; }

        public virtual DbSet<Line> Lines { get; set; }

        public virtual DbSet<Node> Nodes { get; set; }

        public virtual DbSet<Venue> Venues { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //Here I Used the named connection string from appsettings.json
                optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DbConnection"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // This Implementation is the best practice by following the  S Principle from SOLID Principles
            modelBuilder.ApplyConfiguration(new FloorConfiguration());
            modelBuilder.ApplyConfiguration(new LineConfiguration());
            modelBuilder.ApplyConfiguration(new NodeConfiguration());
            modelBuilder.ApplyConfiguration(new VenueConfiguration());

        }

    }
}
