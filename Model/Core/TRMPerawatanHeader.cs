using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfiraClinic.Models.Core
{
    public class TRMPerawatanHeader
    {
        [Column(TypeName = "char(25)")]
        [MaxLength(25)]
        [Key]
        public string NomorTransaksi { get; set; } // DIAG/Tahun/Bulan/Nourut

        [Column(TypeName = "char(5)")]
        [MaxLength(5)]
        public string? TRCD { get; set; } // Transaction Code

        [Column(TypeName = "char(5)")]
        [MaxLength(5)]
        public string? TRSC { get; set; } // Transaction Source

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodeLokasi { get; set; } // Kode Lokasi Klinik

        [Column(TypeName = "char(4)")]
        [MaxLength(4)]
        public string TahunTransaksi { get; set; } // Tahun Transaksi Diagnosa

        [Column(TypeName = "char(2)")]
        [MaxLength(2)]
        public string BulanTransaksi { get; set; } // Bulan Transaksi Diagnosa

        [Column(TypeName = "date")]
        public DateTime TanggalTransaksi { get; set; } // Tanggal Transaksi Diagnosa

        [Column(TypeName = "char(25)")]
        [MaxLength(25)]
        public string? NomorAppointment { get; set; } // Nomor Appointment

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? KodeCustomer { get; set; } // Relasi dengan Table Customer


        [Column(TypeName = "char(5)")]
        [MaxLength(5)]
        public string? KodePoli { get; set; } // Relasi dengan Table MDiagnosa

        [Column(TypeName = "char(255)")]
        [MaxLength(255)]
        public string? Keterangan { get; set; } // Nomor Appointment

        [Column(TypeName = "char(1)")]
        [MaxLength(1)]
        public string? StatusKonfirmasi { get; set; } // Nomor Appointment

        [Column(TypeName = "datetime")]
        public DateTime? UPDDT { get; set; } // Date time input

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? USRID { get; set; } // User Input
    }
}
