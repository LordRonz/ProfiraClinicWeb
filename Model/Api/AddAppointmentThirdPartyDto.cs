namespace ProfiraClinic.Models.Api
{
    public class AddAppointmentThirdPartyDto
    {
        public string KodeKlinik { get; set; }
        public string KodePasien { get; set; }
        public string KodeTindakanPerawatan { get; set; }
        public string Keterangan { get; set; }
        public DateTime TanggalAppointment { get; set; }

    }
}
