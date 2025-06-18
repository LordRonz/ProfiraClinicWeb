using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProfiraClinic.Models.Core
{
    [Table("PBarangH")]
    public class BarangHeader
    {
        [Key]
        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodeBarang { get; set; }

        [Column(TypeName = "bigint")]
        public long IDBarangHeader { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string UnitJual { get; set; }

        [Precision(12, 2)]
        public decimal HargaJual { get; set; }

        [Precision(11, 3)]
        public decimal DiscMember { get; set; }

        [Precision(11, 3)]
        public decimal DiscNonMember { get; set; }

        [Precision(12, 2)]
        public decimal HPP { get; set; }

        [Column(TypeName = "char(1)")]
        [MaxLength(1)]
        public string Aktif { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string USRID { get; set; }

        public DateTime? UPDDT { get; set; }
    }
}
