using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfiraClinic.Models.Core
{
    [Table("TRM_Gambar")]
    public class TRMGambar
    {
        [Column(TypeName = "char(25)")]
        [MaxLength(25)]
        [Key]
        public string NomorTransaksi { get; set; }

        [Column(TypeName = "char(5)")]
        [MaxLength(5)]
        public string? TRCD { get; set; }

        [Column(TypeName = "char(5)")]
        [MaxLength(5)]
        public string? TRSC { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodeLokasi { get; set; }

        [Column(TypeName = "char(4)")]
        [MaxLength(4)]
        public string TahunTransaksi { get; set; }

        [Column(TypeName = "char(2)")]
        [MaxLength(2)]
        public string BulanTransaksi { get; set; }

        [Column(TypeName = "date")]
        public DateTime TanggalTransaksi { get; set; }

        [Column(TypeName = "char(25)")]
        [MaxLength(25)]
        public string? NomorAppointment { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? KodeCustomer { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? KodeKaryawan { get; set; }

        [Column(TypeName = "char(5)")]
        [MaxLength(5)]
        public string? KodePoli { get; set; }

        [Column(TypeName = "varchar(1024)")]
        [MaxLength(5)]
        public string? LinkGambar { get; set; }

        [Column(TypeName = "int")]
        public int NomorUrut { get; set; }

        [Column(TypeName = "varchar(1024)")]
        [MaxLength(1024)]
        public string? Keterangan { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? UPDDT { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? USRID { get; set; }
    }
}
