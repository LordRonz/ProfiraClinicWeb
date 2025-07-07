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
    [Table("PPaketD")]
    public class PaketDetail
    {
        [Column(TypeName = "bigint")]
        public long IDPaketHeader { get; set; }

        [Column(TypeName = "bigint")]
        public long IDPaketDetail { get; set; }

        [Key]
        [Column(TypeName = "int")]
        public int NoUrut { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodePaket { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodePerawatan { get; set; }

        [Column(TypeName = "int")]
        public int JumlahPerawatan { get; set; }
    }
}
