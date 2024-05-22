using System.ComponentModel.DataAnnotations;

namespace ProfiraClinicWeb.Models
{
    public class MKary
    {
        [Key]
        public string? UserName { get; set; }
        public string? UserPass { get; set; }
        public string KDKAR { get; set; }
        public string NMKAR { get; set; }
        public DateTime? TGMSK { get; set; }
        public DateTime? TGMBR { get; set; }
        public string ALMT1 { get; set; }
        public string KOTA1 { get; set; }
        public string TELP1 { get; set; }
        public string ALMT2 { get; set; }
        public string KOTA2 { get; set; }
        public string TELP2 { get; set; }
        public string KOTAL { get; set; }
        public DateTime? TGLLH { get; set; }
        public string AKTIF { get; set; }
        public string USRID { get; set; }
        public DateTime? UPDDT { get; set; }
    }
}
