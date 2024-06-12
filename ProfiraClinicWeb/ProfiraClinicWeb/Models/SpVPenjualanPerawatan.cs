using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProfiraClinicWeb.Models
{
    public class SpVPenjualanPerawatan
    {
        [Key]
        public string KDBHN { get; set; }
        public string KETLK { get; set; }
        public string NOKON { get; set; }
        [Precision(12, 2)]
        public decimal HGSAT { get; set; }
        [Precision(12, 2)]
        public decimal DISCO { get; set; }
        [Precision(12, 2)]
        public decimal NILPR { get; set; }
        [Precision(11, 3)]
        public decimal JMLJL { get; set; }
        public DateTime? TGKON { get; set; }
        public string? KDCUS { get; set; }
        public string? UNTJL { get; set; }
        public string? NMCUS { get; set; }
    }
}
