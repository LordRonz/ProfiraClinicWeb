using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProfiraClinicWeb.Models
{
    public class SpVPenjualanPerawatan
    {
        [Key]
        public string KDPER { get; set; }
        public string KETLK { get; set; }
        public string NOKON { get; set; }
        [Precision(12, 2)]
        public decimal JUMLA { get; set; }
        public string? INRWT { get; set; }
        public DateTime? TGKON { get; set; }
        public string? JENIS { get; set; }
        public string? KDCUS { get; set; }
        public string? KDTER { get; set; }
        public string? NMPER { get; set; }
        public string? NMCUS { get; set; }
        public string? NMKAR { get; set; }
    }
}
