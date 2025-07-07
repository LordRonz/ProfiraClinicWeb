using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProfiraClinic.Models
{
    public class AppointmentOld
    {
        [Column(TypeName = "nchar(10)")]
        public string? KdLok { get; set; }

        [Column(TypeName = "char(4)")]
        public string? Tahun { get; set; }

        [Column(TypeName = "char(2)")]
        public string? Bulan { get; set; }

        public DateTime? TgFak { get; set; }

        [Column(TypeName = "char(15)")]
        public string? NoApp { get; set; }

        public DateTime? TgApp { get; set; }
        public DateTime? JmApp { get; set; }

        [Key]
        [Column(TypeName = "nchar(10)")]
        public string? KdTer { get; set; }

        [Column(TypeName = "char(1)")]
        public string? InCus { get; set; }

        [Column(TypeName = "nchar(10)")]
        public string? KdCus { get; set; }

        [Column(TypeName = "nchar(50)")]
        public string? NmCus { get; set; }

        [Column(TypeName = "char(50)")]
        public string? Telp { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Tindakan { get; set; }

        [Column(TypeName = "char(1)")]
        public string? InKat { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Keter { get; set; }

        [Column(TypeName = "char(1)")]
        public string? InApp { get; set; }

        public DateTime? Reception { get; set; }
        public DateTime? Perawatan { get; set; }
        public DateTime? Konfirmasi { get; set; }
        public DateTime? Cashier { get; set; }

        [Column(TypeName = "char(1)")]
        public string? Status { get; set; }

        public DateTime? UpdDt { get; set; }

        [Column(TypeName = "char(10)")]
        public string? UsrId { get; set; }
    }
}
