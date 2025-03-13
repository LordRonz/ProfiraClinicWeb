using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProfiraClinic.Models
{
    public class PPaketH
    {
        public string KDJNS { get; set; }
        public string KDGRP { get; set; }
        [Key]
        public string KDPKT { get; set; }
        public string? NMPKT { get; set; }
        [Precision(12, 2)]
        public decimal HARGA { get; set; }
        [Precision(11, 3)]
        public decimal DISMB { get; set; }
        [Precision(11, 3)]
        public decimal DISNM { get; set; }
        [Precision(11, 3)]
        public decimal MALAK { get; set; }
        public string? AKTIF { get; set; }
        public DateTime? UPDDT { get; set; }
        public string? USRID { get; set; }
    }
}
