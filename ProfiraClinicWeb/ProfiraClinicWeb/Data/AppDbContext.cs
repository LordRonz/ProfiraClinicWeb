using Excubo.Blazor.Canvas.Contexts;
using Microsoft.EntityFrameworkCore;
using ProfiraClinicWeb.Models;
using System.Collections.Generic;

namespace ProfiraClinicWeb.Data
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

        public DbSet<MKARY> MKARY { get; set; }
    }
}
