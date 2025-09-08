namespace ProfiraClinic.Models.Api
{
    public class AddAppointmentThirdPartyDto
    {
        string KodeKlinik { get; set; }
        string KodePasien { get; set; }
        string KodeTindakanPerawatan { get; set; }
        DateTime TanggalAppointment { get; set; }

    }
}
