using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ProfiraClinicWebAPI.Data.Converters;
using ProfiraClinic.Models;
using MCustomer = ProfiraClinic.Models.Core.MCustomer;
using MKaryawan = ProfiraClinic.Models.Core.MKaryawan;
using ProfiraClinic.Models.Core;

namespace ProfiraClinicWebAPI.Data
{
    public class AppDbContext : DbContext
    {
        public IConfiguration _config { get; set; }
        public AppDbContext(IConfiguration config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DatabaseConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply the trim converter to every string property
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(string)))
                {
                    // Only apply if the converter hasn't been set explicitly
                    if (property.GetValueConverter() == null)
                    {
                        property.SetValueConverter(new TrimStringConverter());
                    }
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<MCustomer> MCustomer { get; set; }
        public DbSet<MKaryawan> MKaryawan { get; set; }
        public DbSet<MKlinik> MKlinik { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<PPerawatanH> PPerawatanH { get; set; }
        public DbSet<UserGroup> MUserGroup { get; set; }
        public DbSet<User> MUser { get; set; }
        public DbSet<Referensi> MRefferensi { get; set; }
        public DbSet<GroupPaket> GroupPaket { get; set; }
        public DbSet<GroupPerawatan> GroupPerawatan { get; set; }
    }
}
