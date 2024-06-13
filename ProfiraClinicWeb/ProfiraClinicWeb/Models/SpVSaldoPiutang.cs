using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProfiraClinicWeb.Models
{
    [Keyless]
    public class SpVSaldoPiutang
    {
        public string KETLK { get; set; }
        public string NOFAK { get; set; }
        [Precision(12, 2)]
        public decimal NILBY { get; set; }
        [Precision(12, 2)]
        public decimal NILAI { get; set; }
        [Precision(12, 2)]
        public decimal SISA { get; set; }
        public DateTime? TGFAK { get; set; }
        public string? KDCUS { get; set; }
        public string? NMCUS { get; set; }
    }
}
