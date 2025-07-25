﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfiraClinic.Models.Core
{
    public class TRMPemeriksaanUmum
    {
        [Key]
        [Column(TypeName = "char(25)")]
        [MaxLength(25)]
        public string NomorTransaksi { get; set; } // RMPU/Tahun/Bulan/Nourut

        [Column(TypeName = "char(5)")]
        [MaxLength(5)]
        public string? TRCD { get; set; } // Transaction Code

        [Column(TypeName = "char(5)")]
        [MaxLength(5)]
        public string? TRSC { get; set; } // Transaction Source

        [Column(TypeName = "char(5)")]
        [MaxLength(5)]
        public string KodeLokasi { get; set; } // Kode Lokasi Klinik

        [Column(TypeName = "char(4)")]
        [MaxLength(4)]
        public string TahunTransaksi { get; set; } // Tahun Transaksi Diagnosa

        [Column(TypeName = "char(2)")]
        [MaxLength(2)]
        public string BulanTransaksi { get; set; } // Bulan Transaksi Diagnosa

        [Column(TypeName = "date")]
        public DateTime TanggalTransaksi { get; set; } // Tanggal Transaksi Diagnosa

        [Column(TypeName = "char(25)")]
        [MaxLength(25)]
        public string NomorAppointment { get; set; } // Nomor Appointment

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodeCustomer { get; set; } // Relasi dengan Table Customer

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodeKaryawan { get; set; } // Kode Dokter / Terapis

        [Column(TypeName = "varchar(1024)")]
        [MaxLength(1024)]
        public string KeadaanUmum { get; set; } // Penjelasan Keadaan Pasien secara umum

        [Column(TypeName = "varchar(1024)")]
        [MaxLength(1024)]
        public string TingkatKesadaran { get; set; } // Penjelasan Tingkat Kesadaran

        [Column(TypeName = "varchar(1024)")]
        [MaxLength(1024)]
        public string? Kesadaran { get; set; } // Penjelasan Kesadaran

        public int? Sistolik { get; set; } // Tekanan darah Sistolik
        public int? Distolik { get; set; } // Tekanan darah Distolik
        public int? Suhu { get; set; } // Suhu Pasien
        public int? Saturasi { get; set; } // Saturasi Oksigen Pasien
        public int? FrekuensiNadi { get; set; } // Frekuensi Nadi Pasien
        public int? FrekuensiNapas { get; set; } // Frekuensi Napas Pasien

        [Column(TypeName = "numeric")]
        public decimal? BeratBadan { get; set; } // Berat badan Pasien

        [Column(TypeName = "numeric")]
        public decimal? TinggiBadan { get; set; } // Tinggi Badan Pasien

        [Column(TypeName = "numeric")]
        public decimal? IndexTubuh { get; set; } // Index Tubuh Pasien

        [Column(TypeName = "numeric")]
        public decimal? LingkarKepala { get; set; } // Lingkar Kepala Pasien

        [Column(TypeName = "datetime")]
        public DateTime? UPDDT { get; set; } // Date time input

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? USRID { get; set; } // User Input

        public string? KETLK { get; set; }
        public string? NamaCustomer { get; set; }

        public string? NamaKaryawan { get; set; }
    }
}
