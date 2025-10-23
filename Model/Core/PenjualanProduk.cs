using Microsoft.EntityFrameworkCore;

namespace ProfiraClinic.Models.Core
{
    [Keyless]
    public class PenjualanProduk
    {
        public string? KETLK { get; set; }

        public string? NOFAK { get; set; }

        public DateTime? TGFAK { get; set; }

        public string? KDCUS { get; set; }

        public string? KDBHN { get; set; }

        public decimal? JMLJL { get; set; }

        public string? UNTJL { get; set; }

        public decimal? HGSAT { get; set; }

        public decimal? DISCO { get; set; }

        public decimal? NILPR { get; set; }
    }
}
