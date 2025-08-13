using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfiraClinic.Models.Api
{
    public class AddRiwayatDto
    {
        public string? KodeLokasi { get; set; }
        public DateTime? TanggalTransaksi { get; set; }
        public string? NomorAppointment { get; set; }
        public string? KodeCustomer { get; set; }
        public string? KodeKaryawan { get; set; }
        public string? PenyakitDahulu { get; set; }
        public string? chkPenyakit { get; set; }
        public string? PenyakitSekarang { get; set; }
        public string? chkAlergiObat { get; set; }
        public string? KetAlergiObat { get; set; }
        public string? chkAlergiMakanan { get; set; }
        public string? KetAlergiMakanan { get; set; }
        public string? chkResiko { get; set; }
        public string? KetResiko { get; set; }
    }
}
