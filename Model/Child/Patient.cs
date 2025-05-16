using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProfiraClinic.Models.Child
{
    [Keyless]
    [Table("MCUST")]
    public class Patient
    {
        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodeCustomer { get; set; }

        [Column(TypeName = "varchar(255)")]
        [MaxLength(255)]
        public string NamaCustomer { get; set; }

        [Column(TypeName = "char(1)")]
        [MaxLength(1)]
        public string? JenisKelamin { get; set; }

        [Column(TypeName = "char(50)")]
        [MaxLength(50)]
        public string? TempatLahir { get; set; }

        public DateTime? TanggalLahir { get; set; }

        [Column(TypeName = "char(2)")]
        [MaxLength(2)]
        public string? GolonganDarah { get; set; }

        [Column(TypeName = "char(255)")]
        [MaxLength(255)]
        public string? AlamatDomisili { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string? KotaDomisili { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string? NomorHP { get; set; }

        [Column(TypeName = "char(1)")]
        [MaxLength(1)]
        public string? StatusMenikah { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? Agama { get; set; }

        [Column(TypeName = "char(50)")]
        [MaxLength(50)]
        public string? Hobbi { get; set; }

        [Column(TypeName = "char(15)")]
        [MaxLength(15)]
        public string? Pendidikan { get; set; }

        [Column(TypeName = "char(50)")]
        [MaxLength(50)]
        public string? Profesi { get; set; }

        [Column(TypeName = "char(50)")]
        [MaxLength(50)]
        public string? Referensi { get; set; }

        [Column(TypeName = "varchar(255)")]
        [MaxLength(255)]
        public string? Note { get; set; }

        [Column(TypeName = "char(1)")]
        [MaxLength(1)]
        public string? AKTIF { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? USRID { get; set; }
    }
}
