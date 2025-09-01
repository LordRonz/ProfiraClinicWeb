namespace ProfiraClinic.Models.Api
{
    public class AppointmentRequest()
    {
        public string? KodeLokasi { get; set; }
        public DateTime? TanggalAppointment { get; set; }
        public string? KodeKaryawan { get; set; }
    }
}
