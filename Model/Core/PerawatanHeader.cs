using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfiraClinic.Models.Core
{
    [Table("PPerawatanH")]
    public class PerawatanHeader
    {
        [Column]
        public long IDPerawatanHeader { get; set; }

        [Column(TypeName = "char(10)")]
        public string KodeJenis { get; set; }

        [Column(TypeName = "char(10)")]
        public string KodeGroupPerawatan { get; set; }

        [Column(TypeName = "char(10)")]
        public string? KategoriPerawatan { get; set; }

        [Key]
        [Column(TypeName = "char(10)")]
        public string KodePerawatan { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string NamaPerawatan { get; set; } = "";

        [Column(TypeName = "numeric(12,2)")]
        [Required]
        public decimal Harga { get; set; } = 0;

        [Column(TypeName = "numeric(11,3)")]
        [Required]
        public decimal DiscMember { get; set; } = 0;

        [Column(TypeName = "numeric(11,3)")]
        [Required]
        public decimal DiscNonMember { get; set; } = 0;

        [Required]
        public int Point { get; set; } = 0;

        [Column(TypeName = "char(1)")]
        public string? Aktif { get; set; }

        public DateTime? UpdDt { get; set; }

        [Column(TypeName = "char(10)")]
        public string? UsrId { get; set; }
    }
}
