using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfiraClinic.Models.Core
{
    [Table("MJABATAN")]
    public class Jabatan
    {
        [Key]
        [Column(TypeName = "char(5)")]
        [MaxLength(5)]
        public string KodeJabatan { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string? NamaJabatan { get; set; }

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
