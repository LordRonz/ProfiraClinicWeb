using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfiraClinic.Models.Core
{
    [Keyless]
    [Table("MUserGroupOtorisasi")]
    public class UserGroupOtorisasi
    {

        [Column(TypeName = "char(5)")]
        [MaxLength(5)]
        public string KodeAplikasi { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodeUserGroup { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string KodeModul { get; set; }

        public DateTime? UPDDT { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string USRID { get; set; }
    }
}
