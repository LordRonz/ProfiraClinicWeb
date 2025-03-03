using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProfiraClinicWebAPI.Model
{
    // TINDAKAN
    public class PPerawatanH
    {
        [Column(TypeName = "char(10)")]
        public string KodeJenis { get; set; }

        [Column(TypeName = "char(10)")]
        public string KodeGroup { get; set; }

        [Key]
        [Column(TypeName = "char(10)")]
        public string KodePerawatan { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? NamaPerawatan { get; set; }

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
