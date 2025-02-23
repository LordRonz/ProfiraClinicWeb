using System.ComponentModel.DataAnnotations;

namespace ProfiraClinicWebAPI.Model
{
    public class MKlinik
    {
        [Key]
        public string? KDLOK { get; set; }
        public string? KETLK { get; set; }
        public string? ALAMAT { get; set; }
        public string? KOTA { get; set; }
        public string? TELP { get; set; }
        public string? NAMAPT { get; set; }
        public string? ALAMATPT { get; set; }
        public string? KOTAPT { get; set; }
        public string? NPWPPT { get; set; }
        public DateTime? UPDDT { get; set; }
        public string? USRID { get; set; }
        public string? DBName { get; set; }
        public string? BranchID { get; set; }
    }
}
