using System.ComponentModel.DataAnnotations;

namespace ProfiraClinicWebAPI.Model
{
    public class MCustomer
    {
        [Key]
        public string KDCUS { get; set; }
        public string NMCUS { get; set; }
        public string? SEXCS { get; set; }
        public DateTime? TGLHR { get; set; }
        public string? GOLDarah { get; set; }
        public string? ALAMAT { get; set; }
        public string? KOTA { get; set; }
        public string? WNegara { get; set; }
        public string? NIK { get; set; }
        public string? EMAIL { get; set; }
        public string? TELP { get; set; }
        public string? NOMHP { get; set; }
        public string? MARIT { get; set; }
        public string? AGAMA { get; set; }
        public string? HOBBI { get; set; }
        public string? PENDI { get; set; }
        public string? PROFESI { get; set; }
        public string? REFF { get; set; }
        public string? NOTE { get; set; }
        public string? FOTO { get; set; }
        public string? INMBR { get; set; }
        public string? NOMBR { get; set; }
        public DateTime? TGMBR { get; set; }
        public string? AKTIF { get; set; }
        public DateTime? UPDDT { get; set; }
        public string? USRID { get; set; }
        public DateTime? TGREG { get; set; }
        public string? KLINIK { get; set; }
    }
}
