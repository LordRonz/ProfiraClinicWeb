using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfiraClinic.Models.Core
{
    [Table("MDiagnosa")]
    [Keyless]
    public class MasterDiagnosa
    {
        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? KodeDiagnosa { get; set; } // Kategori: Primary / Secondary

        [Column(TypeName = "char(255)")]
        [MaxLength(255)]
        public string? NamaDiagnosa { get; set; } // Keterangan Diagnosa

        [Column(TypeName = "datetime")]
        public DateTime? UPDDT { get; set; } // Date time input

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? USRID { get; set; } // User Input
    }
}
