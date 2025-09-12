using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfiraClinic.Models.Core
{
    [Table("PPerawtanD")]
    public class PerawatanDetail
    {
        [Column(TypeName = "bigint")]
        public long IDPerawatanHeader { get; set; }

        [Column(TypeName = "bigint")]
        public long IDPerawatanDetail { get; set; }

        [Key]
        [Column(TypeName = "int")]
        public int NoUrut { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodeBarang { get; set; }

        [Precision(11, 3)]
        public decimal JumlahPakai { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string UnitPakai { get; set; }

        [Precision(11, 3)]
        public decimal JumlahUnitDasar { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string UnitDasar { get; set; }

        [Column(TypeName = "char(1)")]
        [MaxLength(1)]
        public string IndikatorPakai { get; set; }

        [Precision(18, 2)]
        public decimal HargaSatuan { get; set; }

        [Precision(18, 2)]
        public decimal Nilai { get; set; }
    }
}
