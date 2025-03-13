using System.ComponentModel.DataAnnotations;

namespace ProfiraClinic.Models
{
    public class MCustomer
    {
        [Key]
        public string KodeCustomer { get; set; }
        public string NamaCustomer { get; set; }
        public string? JenisKelamin { get; set; }
        public DateTime? TanggalLahir { get; set; }
        public string? GolonganDarah { get; set; }
        public string? ALAMAT { get; set; }
        public string? KOTA { get; set; }
        public string? WargaNegara { get; set; }
        public string? NIK { get; set; }
        public string? EMAIL { get; set; }
        public string? TELP { get; set; }
        public string? NomorHP { get; set; }
        public string? StatusMenikah { get; set; }
        public string? AGAMA { get; set; }
        public string? HOBBI { get; set; }
        public string? Pendidikan { get; set; }
        public string? PROFESI { get; set; }
        public string? Referrensi { get; set; }
        public string? NOTE { get; set; }
        public string? FOTO { get; set; }
        public string? IndikatorMember { get; set; }
        public string? NomorMember { get; set; }
        public DateTime? TanggalMember { get; set; }
        public string? AKTIF { get; set; }
        public DateTime? UPDDT { get; set; }
        public string? USRID { get; set; }
        public DateTime? TanggalRegistrasi { get; set; }
        public string? KLINIK { get; set; }
    }
}
