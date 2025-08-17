using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfiraClinic.Models.Core
{
    [Table("TRM_PenandaanGambar_Detail")]
    public class TRMPenandaanGambarDetail
    {
        [Key]
        [Column(TypeName = "char(25)")]
        [MaxLength(25)]
        public string NomorTransaksi { get; set; } // RMPU/Tahun/Bulan/Nourut

        [Column(TypeName = "char(25)")]
        [MaxLength(25)]
        public string NomorAppointment { get; set; } // Nomor Appointment

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodeGambar { get; set; } // Relasi dengan Table Customer

        [Column(TypeName = "char(255)")]
        [MaxLength(255)]
        public string IDGambar { get; set; } // Kode Dokter / Terapis
    }
}
