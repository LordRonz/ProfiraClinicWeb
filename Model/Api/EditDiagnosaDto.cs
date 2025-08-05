using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfiraClinic.Models.Api
{
    public class EditDiagnosaDto
    {
        public string? KodeLokasi { get; set; }
        public DateTime? TanggalTransaksi { get; set; }
        public string? NomorAppointment { get; set; }
        public string? KodeCustomer { get; set; }
        public string? KodeKaryawan { get; set; }
        public string? KodeDiagnosa { get; set; }
        public string? KategoriDiagnosa { get; set; }
        public string? KeteranganDiagnosa { get; set; }
        public string NomorTransaksi { get; set; } = default!;
    }

}
