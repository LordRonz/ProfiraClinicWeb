using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfiraClinic.Models.Api
{
    public class CreateBarangItemDto
    {
        public string KodeBarang { get; set; }
        public string KodeGroupBarang { get; set; }
        public string NamaBarang { get; set; }
        public string KeteranganBarang { get; set; }
        public string Unit1 { get; set; }
        public string Unit2 { get; set; }
        public string UnitDasar { get; set; }
        public decimal KonversiUnit1 { get; set; }
        public decimal KonversiUnit2 { get; set; }
        public string Aktif { get; set; } = "1";
        public string USRID { get; set; }
    }
}
