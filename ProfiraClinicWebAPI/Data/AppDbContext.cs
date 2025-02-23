using Microsoft.EntityFrameworkCore;
using ProfiraClinicWebAPI.Model;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

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

        public DbSet<MCustomer> MCustomer { get; set; }
        public DbSet<MKARY> MKARY { get; set; }
        public DbSet<MKlinik> MKlinik { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
    }
}
