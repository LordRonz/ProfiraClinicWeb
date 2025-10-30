using Microsoft.EntityFrameworkCore;

namespace ProfiraClinic.Models.Core
{
    [Keyless]
    public class SaldoPaket
    {
        public string KETLK { get; set; }

        public string NOFAK { get; set; }

        public string KDCUS { get; set; }

        public string NMCUS { get; set; }

        public string KDPKT { get; set; }

        public string NMPKT { get; set; }

        public string KDPER { get; set; }

        public string NMPER { get; set; }

        public int FREKW { get; set; }

        public int JMLPR { get; set; }

        public int SISA { get; set; }

        public DateTime? TGAKH { get; set; }
    }
}
