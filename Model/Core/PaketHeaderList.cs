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
    public class PaketHeaderList
    {
        [Key]
        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodePaket { get; set; }

        [Column(TypeName = "bigint")]
        public long IDPaketHeader { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodeJenis { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodeGroupPaket { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string NamaPaket { get; set; }

        [Precision(12, 2)]
        public decimal HARGA { get; set; }

        [Precision(11, 3)]
        public decimal DiscMember { get; set; }

        [Precision(11, 3)]
        public decimal DiscNonMember { get; set; }

        [Precision(11, 3)]
        public decimal MasaLaku { get; set; }

        [Column(TypeName = "char(1)")]
        [MaxLength(1)]
        public string Aktif { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string USRID { get; set; }

        public DateTime? UPDDT { get; set; }

        public string? NamaGroupPaket { get; set; }
    }
}
