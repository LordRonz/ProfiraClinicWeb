using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfiraClinic.Models.Core
{
    public class MKaryawan
    {
        [Key]
        public long IDKaryawan { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodeKaryawan { get; set; }

        [Column(TypeName = "char(255)")]
        [MaxLength(255)]
        public string NamaKaryawan { get; set; }

        [Column(TypeName = "char(20)")]
        [MaxLength(20)]
        public string JenisKelamin { get; set; }

        [Column(TypeName = "char(50)")]
        [MaxLength(50)]
        public string TempatLahir { get; set; }

        public DateTime TanggalLahir { get; set; }

        [Column(TypeName = "char(2)")]
        [MaxLength(2)]
        public string GolonganDarah { get; set; }

        [Column(TypeName = "char(255)")]
        [MaxLength(255)]
        public string Alamat { get; set; }

        [Column(TypeName = "char(50)")]
        [MaxLength(50)]
        public string Kota { get; set; }

        [Column(TypeName = "char(3)")]
        [MaxLength(3)]
        public string WargaNegara { get; set; }

        [Column(TypeName = "char(20)")]
        [MaxLength(20)]
        public string NIK { get; set; }

        [Column(TypeName = "char(255)")]
        [MaxLength(255)]
        public string Email { get; set; }

        [Column(TypeName = "char(50)")]
        [MaxLength(50)]
        public string NomorHP { get; set; }

        [Column(TypeName = "char(20)")]
        [MaxLength(20)]
        public string StatusMenikah { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string Agama { get; set; }

        [Column(TypeName = "char(255)")]
        [MaxLength(255)]
        public string Foto { get; set; }

        [Column(TypeName = "char(15)")]
        [MaxLength(15)]
        public string Pendidikan { get; set; }

        [Column(TypeName = "char(1)")]
        [MaxLength(1)]
        public string AKTIF { get; set; }

        public DateTime? UPDDT { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string USRID { get; set; }

        public DateTime? TglRegistrasi { get; set; }

        [Column(TypeName = "char(5)")]
        [MaxLength(5)]
        public string KodeLokasi { get; set; }
    }

}
