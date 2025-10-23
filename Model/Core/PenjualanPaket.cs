using Microsoft.EntityFrameworkCore;

namespace ProfiraClinic.Models.Core
{
    [Keyless]
    public class PenjualanPaket
    {
        public string? KETLK { get; set; }

        public string? NOFAK { get; set; }

        public DateTime? TGFAK { get; set; }

        public string? KDCUS { get; set; }

        public string? NMCUS { get; set; }

        public string? KDPKT { get; set; }

        public string? NMPKT { get; set; }

        public decimal? JUMLA { get; set; }
    }
}
