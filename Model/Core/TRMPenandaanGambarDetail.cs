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

        public int IDDetail { get; set; } // IDDetail

        [Column(TypeName = "char(255)")]
        [MaxLength(255)]
        public string KodeGambar { get; set; } // Relasi dengan Table Customer

        [Column(TypeName = "char(255)")]
        [MaxLength(255)]
        public string IDGambar { get; set; } // Kode Dokter / Terapis
    }
}
