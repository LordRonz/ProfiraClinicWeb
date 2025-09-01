using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfiraClinic.Models.Api
{
    public class EditPenandaanGambarHeaderDto
    {
        public string NomorTransaksi { get; set; } // DIAG/Tahun/Bulan/Nourut

        public DateTime TanggalTransaksi { get; set; } // Tanggal Transaksi Diagnosa

        [Column(TypeName = "char(25)")]
        [MaxLength(25)]
        public string? NomorAppointment { get; set; } // Nomor Appointment

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? KodeCustomer { get; set; } // Relasi dengan Table Customer

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? KodeKaryawan { get; set; }

        [Column(TypeName = "varchar(1024)")]
        [MaxLength(1024)]
        public string? Keterangan { get; set; } // Keterangan
    }
}
