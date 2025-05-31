using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfiraClinic.Models.Core
{
    public class DokterListDto
    {
        public string KodeLokasi { get; set; } = null!;
        public string Lokasi { get; set; } = null!;
        public string KodeKaryawan { get; set; } = null!;
        public string NamaDokter { get; set; } = null!;
        public string KodeJenisDokter { get; set; } = null!;
        public string NamaJenisDokter { get; set; } = null!;
        public string KodeJabatan { get; set; } = null!;
        public string NamaJabatan { get; set; } = null!;
        public string AKTIF { get; set; }
        public DateTime UPDDT { get; set; }
        public string USRID { get; set; } = null!;
        public string FOTO { get; set; }
    }
}
