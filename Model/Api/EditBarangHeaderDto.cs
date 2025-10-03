namespace ProfiraClinic.Models.Api
{

    public class EditBarangHeaderDto
    {
        public string KodeBarang { get; set; }
        public string UnitJual { get; set; }
        public string HargaJual { get; set; }
        public string DiscMember { get; set; }
        public string DiscNonMember { get; set; }
        public string Aktif { get; set; } = "1";
        public string USRID { get; set; }
    }
}
