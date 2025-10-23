using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfiraClinic.Models.Core
{
    [Table("TRM_Perawatan_Detail")]
    public class TRMPerawatanDetail
    {
        [Key]
        public int IdDetail { get; set; }

        [Column(TypeName = "char(25)")]
        [MaxLength(25)]
        public string NomorTransaksi { get; set; } // DIAG/Tahun/Bulan/Nourut

        [Column(TypeName = "int")]
        public int? NomorUrut { get; set; } = 0;

        [Column(TypeName = "char(1)")]
        [MaxLength(1)]
        public string? JenisPerawatan { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? KodePaket { get; set; }

        [Column(TypeName = "char(21)")]
        [MaxLength(21)]
        public string? NomorFakturPaket { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? KodePerawatan { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string NamaPerawatan { get; set; } = "";

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? KodePerawatanPengganti { get; set; }

        public int Qty { get; set; }

        [Column(TypeName = "varchar(255)")]
        [MaxLength(255)]
        public string? KeteranganDetail { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? KodeDokter { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? KodePerawat1 { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? KodePerawat2 { get; set; }
    }
}
