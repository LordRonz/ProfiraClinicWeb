using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfiraClinic.Models.Api
{
    public class AddCPPTDto
    {
        public string? KodeLokasi { get; set; }
        public DateTime? TanggalTransaksi { get; set; }
        public string? NomorAppointment { get; set; }
        public string? KodeCustomer { get; set; }
        public string? KodeKaryawan { get; set; }
        public string? SUBYEKTIF { get; set; }
        public string? OBYEKTIF { get; set; }
        public string? ASSESTMENT { get; set; }
        public string? PLANNING { get; set; }
        public string? INSTRUKSI { get; set; }
        public string? INPMD { get; set; }  // e.g. 'A' or other code
    }
}
