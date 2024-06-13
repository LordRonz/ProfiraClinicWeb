using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProfiraClinicWeb.Models
{
    [Keyless]
    public class SpVSaldoPaket
    {
        public string KDPKT { get; set; }
        public string KETLK { get; set; }
        public string NOFAK { get; set; }
        [Precision(12, 2)]
        public decimal JMLPR { get; set; }
        [Precision(12, 2)]
        public decimal FREKW { get; set; }
        [Precision(12, 2)]
        public decimal SISA { get; set; }
        public DateTime? TGAKH { get; set; }
        public string? KDPER { get; set; }
        public string? NMPER { get; set; }
        public string? KDCUS { get; set; }
        public string? NMPKT { get; set; }
        public string? NMCUS { get; set; }
    }
}
