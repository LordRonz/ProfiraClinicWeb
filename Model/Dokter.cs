using System.ComponentModel.DataAnnotations;

namespace ProfiraClinic.Models
{
    public class MKaryawan
    {
        [Key]
        public string? Ref_USRID { get; set; }
        public string KodeKaryawan { get; set; }
        public string NamaKaryawan { get; set; }
        public DateTime? TanggalMasuk { get; set; }
        public string Alamat1 { get; set; }
        public string KOTA1 { get; set; }
        public string TELP1 { get; set; }
        public string Alamat2 { get; set; }
        public string KOTA2 { get; set; }
        public string TELP2 { get; set; }
        public string KotaLahir { get; set; }
        public DateTime? TanggalLahir { get; set; }
        public string AKTIF { get; set; }
        public string USRID { get; set; }
        public DateTime? UPDDT { get; set; }
    }
}
