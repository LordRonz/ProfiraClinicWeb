using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfiraClinic.Models.Core
{
    [Table("PDokter")]
    public class Dokter
    {
        [Key]
        [Column(TypeName = "int")]
        public int IDDokter { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? KodeKaryawan { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? KodeJabatan { get; set; }

        [Column(TypeName = "char(5)")]
        [MaxLength(5)]
        public string KodeJenisDokter { get; set; }

        [Column(TypeName = "varchar(255)")]
        [MaxLength(255)]
        public string? Keterangan { get; set; }

        [Column(TypeName = "varchar(255)")]
        [MaxLength(255)]
        public string? FOTO { get; set; }

        [Column(TypeName = "numeric(12,2)")]
        public decimal? PROverheadKlinik { get; set; }

        [Column(TypeName = "numeric(12,2)")]
        public decimal? PRFeeDokter { get; set; }

        [Column(TypeName = "numeric(12,2)")]
        public decimal? PRFeeOwner { get; set; }

        [Column(TypeName = "numeric(12,2)")]
        public decimal? PRInsentifDokter { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? KodeLokasi { get; set; }

        public DateTime? UPDDT { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string USRID { get; set; }

        [Column(TypeName = "char(1)")]
        [MaxLength(1)]
        public string AKTIF { get; set; }
    }
}
