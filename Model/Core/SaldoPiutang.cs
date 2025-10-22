using Microsoft.EntityFrameworkCore;

namespace ProfiraClinic.Models.Core
{
    [Keyless]
    public class SaldoPiutang
    {
        public string? KETLK { get; set; }

        public string? NOFAK { get; set; }

        public DateTime? TGFAK { get; set; }

        public string? KDCUS { get; set; }

        public string? NMCUS { get; set; }

        public decimal? NILAI { get; set; }

        public decimal? NILBY { get; set; }

        public decimal? SISA { get; set; }
    }
}
