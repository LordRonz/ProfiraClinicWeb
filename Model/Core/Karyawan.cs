using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfiraClinic.Models.Core
{
    [Table("MKaryawan")]
    [Keyless]
    public class Karyawan
    {
        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Column(TypeName = "varchar(255)")]
        [MaxLength(255)]
        public string UserPassword { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? KodeKaryawan { get; set; }

        [Column(TypeName = "char(255)")]
        [MaxLength(255)]
        public string NamaKaryawan { get; set; }

        public DateTime TanggalLahir { get; set; }


        [Column(TypeName = "char(50)")]
        [MaxLength(50)]
        public string Alamat1 { get; set; }

        [Column(TypeName = "char(50)")]
        [MaxLength(50)]
        public string Alamat2 { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string Agama { get; set; }

        [Column(TypeName = "char(1)")]
        [MaxLength(1)]
        public string AKTIF { get; set; }

        public DateTime? UPDDT { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string USRID { get; set; }
    }

}
