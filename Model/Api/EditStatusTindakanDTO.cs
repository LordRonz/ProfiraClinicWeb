using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfiraClinic.Models.Api
{
    public class EditStatusTindakanDTO()
    {
        public string KodeLokasi { get; set; } = "";
        public DateTime? TanggalAppointment { get; set; } 
        public string NomorAppointment { get; set; } = "";
        public string KodeCustomer { get; set; } = "";
    }
}
