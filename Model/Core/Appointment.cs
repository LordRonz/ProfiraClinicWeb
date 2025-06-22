using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProfiraClinic.Models.Core
{
    public class Appointment
    {
        [Column(TypeName = "bigint")]
        public long IDAppointment { get; set; }

        [Column(TypeName = "nchar(10)")]
        public string? KodeLokasi { get; set; }

        [Column(TypeName = "char(4)")]
        public string? TahunTransaksi { get; set; }

        [Column(TypeName = "char(2)")]
        public string? BulanTransaksi { get; set; }

        public DateTime? TgFak { get; set; }

        [Column(TypeName = "char(15)")]
        public string? NomorAppointment { get; set; }

        public DateTime? TanggalAppointment { get; set; }
        public DateTime? JamAppointment { get; set; }

        [Key]
        [Column(TypeName = "nchar(10)")]
        public string? KodeKaryawan { get; set; }

        [Column(TypeName = "char(1)")]
        public string? InCus { get; set; }

        [Column(TypeName = "nchar(10)")]
        public string? KodeCustomer { get; set; }

        [Column(TypeName = "nchar(50)")]
        public string? NamaCustomer { get; set; }

        [Column(TypeName = "char(50)")]
        public string? NomorHP { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Tindakan { get; set; }

        [Column(TypeName = "char(1)")]
        public string? InKat { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Keterangan { get; set; }

        [Column(TypeName = "char(1)")]
        public string? InApp { get; set; }

        public DateTime? Reception { get; set; }
        public DateTime? Perawatan { get; set; }
        public DateTime? Konfirmasi { get; set; }
        public DateTime? Cashier { get; set; }

        [Column(TypeName = "char(1)")]
        public string? StatusTindakan { get; set; }

        [Column(TypeName = "char(1)")]
        public string? StatusAppointment { get; set; }

        public DateTime? UpdDt { get; set; }

        [Column(TypeName = "char(10)")]
        public string? UsrId { get; set; }
    }
}
