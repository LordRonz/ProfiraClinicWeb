using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProfiraClinicWeb.Models
{
    public class SpVPenjualanPaket
    {
        [Key]
        public string KDPKT { get; set; }
        public string KETLK { get; set; }
        public string NOFAK { get; set; }
        [Precision(12, 2)]
        public decimal JUMLA { get; set; }
        public DateTime? TGFAK { get; set; }
        public string? KDCUS { get; set; }
        public string? NMPKT { get; set; }
        public string? NMCUS { get; set; }
    }
}
