using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfiraClinic.Models.Core
{

    public class MCustomer
    {
        [Key]
        public long IDCustomer { get; set; }

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

        [Column(TypeName = "char(3)")]
        [MaxLength(3)]
        public string? RTDomisili { get; set; }

        [Column(TypeName = "char(3)")]
        [MaxLength(3)]
        public string? RWDomisili { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string? KelurahanDomisili { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string? KecamatanDomisili { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string? KotaDomisili { get; set; }

        [Column(TypeName = "char(5)")]
        [MaxLength(5)]
        public string? KodePosDomisili { get; set; }

        [Column(TypeName = "char(3)")]
        [MaxLength(3)]
        public string? WargaNegara { get; set; }

        [Column(TypeName = "varchar(255)")]
        [MaxLength(255)]
        public string? Email { get; set; }

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

        [Column(TypeName = "char(20)")]
        [MaxLength(20)]
        public string? NIK { get; set; }

        [Column(TypeName = "varchar(255)")]
        [MaxLength(255)]
        public string? AlamatNIK { get; set; }

        [Column(TypeName = "char(3)")]
        [MaxLength(3)]
        public string? RTNIK { get; set; }

        [Column(TypeName = "char(3)")]
        [MaxLength(3)]
        public string? RWNIK { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string? KelurahanNIK { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string? KecamatanNIK { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string? KotaNIK { get; set; }

        [Column(TypeName = "char(5)")]
        [MaxLength(5)]
        public string? KodePosNIK { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string? HubunganKeluarga { get; set; }

        [Column(TypeName = "varchar(255)")]
        [MaxLength(255)]
        public string? NamaKeluarga { get; set; }

        [Column(TypeName = "varchar(255)")]
        [MaxLength(255)]
        public string? AlamatKeluarga { get; set; }

        [Column(TypeName = "char(3)")]
        [MaxLength(3)]
        public string? RTKeluarga { get; set; }

        [Column(TypeName = "char(3)")]
        [MaxLength(3)]
        public string? RWKeluarga { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string? KelurahanKeluarga { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string? KecamatanKeluarga { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string? KotaKeluarga { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string? NomorHPKeluarga { get; set; }

        [Column(TypeName = "char(5)")]
        [MaxLength(5)]
        public string? KodePosKeluarga { get; set; }

        [Column(TypeName = "varchar(255)")]
        [MaxLength(255)]
        public string? Note { get; set; }

        [Column(TypeName = "char(1)")]
        [MaxLength(1)]
        public string? StatusMember { get; set; }

        [Column(TypeName = "varchar(21)")]
        [MaxLength(21)]
        public string? NomorMember { get; set; }

        public DateTime? TanggalMember { get; set; }

        [Column(TypeName = "char(1)")]
        [MaxLength(1)]
        public string? AKTIF { get; set; }

        public DateTime? UPDDT { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? USRID { get; set; }

        public DateTime? TanggalRegistrasi { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? KodeLokasi { get; set; }
    }

}
