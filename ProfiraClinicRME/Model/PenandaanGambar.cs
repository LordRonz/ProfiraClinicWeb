namespace ProfiraClinicRME.Model
{
    public class PenandaanGambarFull: PenandaanGambarHeader
    {

        public List<PenandaanGambarDetail> Details { get; set; } = new List<PenandaanGambarDetail>();
    }

    public class PenandaanGambarHeader
    {
        public string KodeLokasi { get; set; } = "";
        public DateTime TanggalTransaksi { get; set; } = DateTime.Now;
        public string NomorAppointment { get; set; } = "";
        public string KodeCustomer { get; set; } = "";
        public string KodeKaryawan { get; set; } = "";
        public string NomorTransaksi { get; set; } = "";
        public string Keterangan { get; set; } = "";
    }

    public class PenandaanGambarDetail
    {
        public int IdDetail { get; set; } = 0;
        public string IdGambar { get; set; } = "";
        public string KodeGambar { get; set; } = "";
    }

}
