namespace ProfiraClinic.Models.Api
{
    public class AddTRMPerawatanHeaderDto
    {
        public string? KodeLokasi { get; set; }          // optional override, otherwise taken from token/user
        public DateTime? TanggalTransaksi { get; set; }  // if null, default = DateTime.Today
        public string? NomorAppointment { get; set; }
        public string? KodeCustomer { get; set; }
        public string? KeteranganTRMPerawatanHeader { get; set; } // maps to @Keterangan
    }
}
