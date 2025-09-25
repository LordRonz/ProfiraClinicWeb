using ProfiraClinic.Models.Core;

namespace ProfiraClinicRME.Model
{
    public class PenandaanGambarFull: TRMPenandaanGambarHeader
    {


        public List<TRMPenandaanGambarDetail> Detail { get; set; } = [];
    }


}
