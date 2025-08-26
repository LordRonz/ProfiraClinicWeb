using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfiraClinic.Models.Api
{
    public class EditPenandaanGambarHeaderDto
        ()
    {
        public string KodeLokasi { get; set; } = "";
        public DateTime TanggalTransaksi { get; set; } = DateTime.Today;
        public string NomorAppointment { get; set; } = "";
        public string KodeCustomer { get; set; } = "";
        public string KodeKaryawan { get; set; } = "";
        public string Keterangan { get; set; } = "";
        public string NomorTransaksi { get; set; } = "";

    }
}
