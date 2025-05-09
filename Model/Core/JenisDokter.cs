using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfiraClinic.Models.Core
{
    [Table("MJenisDokter")]
    public class JenisDokter
    {
        [Key]
        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodeJenisDokter { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string? NamaJenisDokter { get; set; }

        /// <summary>
        /// User yang memasukkan data
        /// </summary>
        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? USRID { get; set; }

        /// <summary>
        /// Tanggal dan waktu update terakhir
        /// </summary>
        public DateTime? UPDDT { get; set; }
    }
}
