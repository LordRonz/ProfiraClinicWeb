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
    [Keyless]
    [Table("MBarang")]
    public class Barang
    {
        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodeBarang { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodeGroupBarang { get; set; }

        [Column(TypeName = "bigint")]
        public long IDBarang { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string NamaBarang { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string KeteranganBarang { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string Unit1 { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string Unit2 { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string UnitDasar { get; set; }

        [Precision(10, 3)]
        public decimal KonversiUnit1 { get; set; }

        [Precision(10, 3)]
        public decimal KonversiUnit2 { get; set; }

        [Column(TypeName = "char(1)")]
        [MaxLength(1)]
        public string Aktif { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string USRID { get; set; }

        public DateTime? UPDDT { get; set; }
    }
}
