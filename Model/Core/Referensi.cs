using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProfiraClinic.Models.Core
{
    [Keyless]
    public class Referensi
    {
        [Column(TypeName = "char(30)")]
        [MaxLength(30)]
        public string Refferensi { get; set; }
    }
}
