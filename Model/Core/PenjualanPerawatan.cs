using Microsoft.EntityFrameworkCore;

namespace ProfiraClinic.Models.Core
{
    [Keyless]
    public class PenjualanPerawatan
    {
        public string? KETLK { get; set; }

        public string? NOKON { get; set; }

        public DateTime? TGKON { get; set; }

        public string? KDCUS { get; set; }

        public string? NMCUS { get; set; }

        public string? INRWT { get; set; }

        public string? JENIS { get; set; }

        public string? KDPER { get; set; }

        public string? NMPER { get; set; }

        public decimal? JUMLA { get; set; }

        public string? KDTER { get; set; }

        public string? NMKAR { get; set; }
    }
}
