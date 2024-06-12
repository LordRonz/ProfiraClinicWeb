using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProfiraClinicWeb.Models
{
    public class SpProduk
    {
        [Key]
        public string KDBHN { get; set; }
        public string UNTJL { get; set; }
        [Precision(12, 2)]
        public decimal HRGJL { get; set; }
        [Precision(11, 3)]
        public decimal DISMB { get; set; }
        [Precision(11, 3)]
        public decimal DISNM { get; set; }
        public DateTime? UPDDT { get; set; }
        public string? USRID { get; set; }
        [Precision(12, 2)]
        public decimal HPP { get; set; }
        public string? NMBHN { get; set; }
    }
}
