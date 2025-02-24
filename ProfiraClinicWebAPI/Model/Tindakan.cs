using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProfiraClinicWebAPI.Model
{
    // TINDAKAN
    public class PPERH
    {
        [Column(TypeName = "char(10)")]
        public string KdJns { get; set; }

        [Column(TypeName = "char(10)")]
        public string KdGrp { get; set; }

        [Key]
        [Column(TypeName = "char(10)")]
        public string KdPer { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? NmPer { get; set; }

        [Column(TypeName = "numeric(12,2)")]
        [Required]
        public decimal Harga { get; set; } = 0;

        [Column(TypeName = "numeric(11,3)")]
        [Required]
        public decimal Dismb { get; set; } = 0;

        [Column(TypeName = "numeric(11,3)")]
        [Required]
        public decimal Disnm { get; set; } = 0;

        [Required]
        public int Point { get; set; } = 0;

        [Column(TypeName = "char(1)")]
        public string? Aktif { get; set; }

        public DateTime? UpdDt { get; set; }

        [Column(TypeName = "char(10)")]
        public string? UsrId { get; set; }
    }
}
