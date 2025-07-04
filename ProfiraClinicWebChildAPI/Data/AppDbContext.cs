using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ProfiraClinic.Models;
using Customer = ProfiraClinic.Models.Core.Customer;
using MKaryawan = ProfiraClinic.Models.Core.MKaryawan;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebChildAPI.Data.Converters;

namespace ProfiraClinicWebChildAPI.Data
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

            modelBuilder.Entity<DokterListDto>()
            .HasNoKey()
            .ToView(null);
        }

        public DbSet<Customer> MCustomer { get; set; }

    }
}
