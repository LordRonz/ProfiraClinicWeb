using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ProfiraClinic.Models;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data.Converters;
using Customer = ProfiraClinic.Models.Core.Customer;
using Karyawan = ProfiraClinic.Models.Core.Karyawan;

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

            modelBuilder.Entity<DokterListDto>()
            .HasNoKey()
            .ToView(null);
        }

        public DbSet<Customer> MCustomer { get; set; }
        public DbSet<Karyawan> MKaryawan { get; set; }
        public DbSet<MKlinik> MKlinik { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<TRMAppointment> AppointmentList { get; set; }
        public DbSet<PPerawatanH> PPerawatanH { get; set; }
        public DbSet<UserGroup> MUserGroup { get; set; }
        public DbSet<User> MUser { get; set; }
        public DbSet<Referensi> MRefferensi { get; set; }
        public DbSet<GroupPaket> GroupPaket { get; set; }
        public DbSet<GroupPerawatan> GroupPerawatan { get; set; }
        public DbSet<GroupBarang> GroupBarang { get; set; }
        public DbSet<CustomerRiwayatAsal> CustomerRiwayatAsal { get; set; }
        public DbSet<Dokter> Dokter { get; set; }
        public DbSet<Jabatan> Jabatan { get; set; }
        public DbSet<JenisDokter> JenisDokter { get; set; }
        public DbSet<DokterListDto> DokterList { get; set; }

        public DbSet<PaketHeader> PaketHeader { get; set; }
        public DbSet<PaketDetail> PaketDetail { get; set; }
        public DbSet<PaketHeaderList> PaketHeaderList { get; set; }
        public DbSet<BarangHeaderList> BarangHeaderList { get; set; }
        public DbSet<BarangHeader> BarangHeader { get; set; }
        public DbSet<Barang> Barang { get; set; }
        public DbSet<BarangListDto> BarangListDto { get; set; }


        public DbSet<Diagnosa> Diagnosa { get; set; }

        public DbSet<TRMDiagnosa> TRMDiagnosa { get; set; }
        public DbSet<PemeriksaanUmum> PemeriksaanUmum { get; set; }
        public DbSet<CPPT> CPPT { get; set; }
        public DbSet<TRMPemeriksaanUmum> TRMPemeriksaanUmum { get; set; }
        public DbSet<TRMCPPT> TRMCPPT { get; set; }
        public DbSet<MasterDiagnosa> MasterDiagnosa { get; set; }

        public DbSet<TRMGambar> TRMGambar { get; set; }
        public DbSet<TRMAnamnesis> TRMAnamnesis { get; set; }
        public DbSet<Anamnesis> Anamnesis { get; set; }

        public DbSet<TRMRiwayat> TRMRiwayat { get; set; }
        public DbSet<Riwayat> Riwayat { get; set; }

        public DbSet<TRMPenandaanGambarHeader> TRMPenandaanGambarHeader { get; set; }
        public DbSet<TRMPenandaanGambarDetail> TRMPenandaanGambarDetail { get; set; }
    }
}
