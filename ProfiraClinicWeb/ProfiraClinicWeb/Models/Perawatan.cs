using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProfiraClinicWeb.Models
{
    public class PPERH
    {
        public string KDJNS { get; set; }
        public string KDGRP { get; set; }
        [Key]
        public string KDPER { get; set; }
        public string? NMPER { get; set; }
        [Precision(12, 2)]
        public decimal HARGA { get; set; }
        [Precision(11, 3)]
        public decimal DISMB { get; set; }
        [Precision(11, 3)]
        public decimal DISNM { get; set; }
        public int POINT { get; set; }
        public string? AKTIF { get; set; }
        public DateTime? UPDDT { get; set; }
        public string? USRID { get; set; }
    }
}
