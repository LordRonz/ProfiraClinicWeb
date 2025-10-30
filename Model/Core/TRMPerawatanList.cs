using Microsoft.EntityFrameworkCore;

namespace ProfiraClinic.Models.Core
{
    [Keyless]
    public class TRMPerawatanList
    {
        public string? KodeLokasi { get; set; }
        public string? NomorAppointment { get; set; }
        public string? NomorTransaksi { get; set; }
        public string? KodeCustomer { get; set; }
        public string? NamaCustomer { get; set; }

        public string? JenisPerawatan { get; set; }
        public long? IDDetail { get; set; }

        public string? KodePaket { get; set; }
        public string? KodePerawatan { get; set; }
        public string? KodeDokter { get; set; }
        public string? KodePerawat1 { get; set; }
        public string? KodePerawat2 { get; set; }
        public string? KeteranganDetail { get; set; }
        public string? NamaKaryawan { get; set; }
    }
}
