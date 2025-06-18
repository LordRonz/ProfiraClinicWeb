using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfiraClinic.Models.Core
{

    public class CurrentUser
    {
        public string UserID { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string? KodeUserGroup { get; set; }
        public string? KodeLokasi { get; set; }
        public MKlinik? Klinik { get; set; }
    }

}
