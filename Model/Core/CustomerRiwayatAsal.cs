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
    [Table("MCustomer_RiwayatAsal")]
    public class CustomerRiwayatAsal
    {
        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? KodeCustomer { get; set; }

        [Column(TypeName = "varchar(255)")]
        [MaxLength(255)]
        public string? PenyakitDahulu { get; set; }

        [Column(TypeName = "char(7)")]
        [MaxLength(7)]
        public string? chkPenyakit { get; set; }

        [Column(TypeName = "varchar(255)")]
        [MaxLength(255)]
        public string? PenyakitSekarang { get; set; }

        [Column(TypeName = "char(2)")]
        [MaxLength(2)]
        public string? chkAlergiObat { get; set; }

        [Column(TypeName = "varchar(255)")]
        [MaxLength(255)]
        public string? KetAlergiObat { get; set; }

        [Column(TypeName = "char(2)")]
        [MaxLength(2)]
        public string? chkAlergiMakanan { get; set; }

        [Column(TypeName = "varchar(255)")]
        [MaxLength(255)]
        public string? KetAlergiMakanan { get; set; }

        [Column(TypeName = "char(4)")]
        [MaxLength(4)]
        public string? chkResiko { get; set; }

        [Column(TypeName = "varchar(255)")]
        [MaxLength(255)]
        public string? KetResiko { get; set; }

    }
}
