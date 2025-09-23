using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfiraClinic.Models.Api
{
    public class EditPenandaanGambarDetailDto
        ()
    {
        public string NomorTransaksi { get; set; }
        public string IdGambar { get; set; } = "";
        public string KodeGambar { get; set; } = "";
    }
}
