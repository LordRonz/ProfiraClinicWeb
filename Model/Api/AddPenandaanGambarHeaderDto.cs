namespace ProfiraClinic.Models.Api
{
    public class AddPenandaanGambarHeaderDto
    {
        public string? KodeLokasi { get; set; }
        public DateTime? TanggalTransaksi { get; set; }
        public string? NomorAppointment { get; set; }
        public string? KodeCustomer { get; set; }
        public string? KodeKaryawan { get; set; }
        public string? NomorUrut { get; set; }
        public string? Keterangan { get; set; }
    }
}
