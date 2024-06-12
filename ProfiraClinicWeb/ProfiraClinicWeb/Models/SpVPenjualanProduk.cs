using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProfiraClinicWeb.Models
{
    public class SpVPenjualanProduk
    {
        [Key]
        public string KDBHN { get; set; }
        public string KETLK { get; set; }
        public string NOFAK { get; set; }
        [Precision(12, 2)]
        public decimal HGSAT { get; set; }
        [Precision(12, 2)]
        public decimal DISCO { get; set; }
        [Precision(12, 2)]
        public decimal NILPR { get; set; }
        [Precision(11, 3)]
        public decimal JMLJL { get; set; }
        public DateTime? TGFAK { get; set; }
        public string? KDCUS { get; set; }
        public string? UNTJL { get; set; }
    }
}
