using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfiraClinic.Models.Core
{
    [Table("MGroupBarang")]
    public class GroupBarang
    {
        [Key]
        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodeGroupBarang { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string NamaGroupBarang { get; set; }

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
