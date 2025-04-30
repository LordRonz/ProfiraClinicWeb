
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ProfiraClinic.Models.Core
{
    [Keyless]
    public class UserGroup
    {

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodeUserGroup { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string NamaUserGroup { get; set; }

        public DateTime? UPDDT { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string USRID { get; set; }
    }
}
