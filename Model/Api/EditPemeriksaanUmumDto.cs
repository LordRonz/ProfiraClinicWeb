namespace ProfiraClinic.Models.Api
{
    public class EditPemeriksaanUmumDto
    {
        public string? KodeLokasi { get; set; }
        public DateTime? TanggalTransaksi { get; set; }
        public string? NomorAppointment { get; set; }
        public string? KodeCustomer { get; set; }
        public string? KodeKaryawan { get; set; }
        public string? Keadaan_Umum { get; set; }
        public string? Tingkat_Kesadaran { get; set; }
        public int Sistolik { get; set; }
        public int Distolik { get; set; }
        public decimal Suhu { get; set; }
        public decimal Saturasi { get; set; }
        public int Frekuensi_Nadi { get; set; }
        public decimal Frekuensi_Nafas { get; set; }
        public decimal BeratBadan { get; set; }
        public decimal TinggiBadan { get; set; }
        public int IndexTubuh { get; set; }
        public decimal LingkarKepala { get; set; }
        public string NomorTransaksi { get; set; } = string.Empty;
    }

}
