using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfiraClinic.Models.Core
{
    [Table("MGroupPerawatan")]
    public class GroupPerawatan
    {
        [Key]
        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodeGroupPerawatan { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string NamaGroupPerawatan { get; set; }

        /// <summary>
        /// Status Aktif: '1' = Aktif, '0' = Non Aktif
        /// </summary>
        [Column(TypeName = "char(1)")]
        [MaxLength(1)]
        public string Aktif { get; set; }

        /// <summary>
        /// User yang memasukkan data
        /// </summary>
        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string USRID { get; set; }

        /// <summary>
        /// Tanggal dan waktu update terakhir
        /// </summary>
        public DateTime? UPDDT { get; set; }
    }
}
