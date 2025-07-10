using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfiraClinic.Models.Core
{
    public class TRMAppointment
    {
        [Column(TypeName = "bigint")]
        public long IDAppointment { get; set; }

        [Column(TypeName = "nchar(10)")]
        public string? KodeLokasi { get; set; }

        [Column(TypeName = "char(4)")]
        public string? TahunTransaksi { get; set; }

        [Column(TypeName = "char(2)")]
        public string? BulanTransaksi { get; set; }

        public DateTime? TanggalTransaksi { get; set; }

        [Key]
        [Column(TypeName = "char(15)")]
        public string NomorAppointment { get; set; }

        public DateTime? TanggalAppointment { get; set; }
        public TimeSpan? JamAppointment { get; set; }

        [Column(TypeName = "nchar(10)")]
        public string? KodeKaryawan { get; set; }

        [Column(TypeName = "nchar(10)")]
        public string? KodeCustomer { get; set; }

        [Column(TypeName = "nchar(50)")]
        public string? NamaCustomer { get; set; }

        [Column(TypeName = "char(50)")]
        public string? NomorHP { get; set; }

        [Column(TypeName = "char(50)")]
        public string? NomorHPCustomer { get; set; }

        public DateTime? TanggalLahir { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? KodeRuangan { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Keterangan { get; set; }

        public DateTime? Reception { get; set; }
        public DateTime? Perawatan { get; set; }
        public DateTime? Konfirmasi { get; set; }
        public DateTime? Cashier { get; set; }

        [Column(TypeName = "char(1)")]
        public string? StatusTindakan { get; set; }

        [Column(TypeName = "char(1)")]
        public string? StatusAppointment { get; set; }

        [Column(TypeName = "int")]
        public int Estimasi { get; set; }

        [Column(TypeName = "int")]
        public int Usia { get; set; }

        public DateTime? UpdDt { get; set; }

        [Column(TypeName = "char(10)")]
        public string? UsrId { get; set; }

        public string? AlamatDomisili { get; set; }
    }
}
