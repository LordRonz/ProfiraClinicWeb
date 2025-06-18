using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProfiraClinic.Models.Core
{
    public class User
    {
        [Key]
        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string UserID { get; set; }

        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Column(TypeName = "varchar(100)")]
        [MaxLength(100)]
        public string Password { get; set; }

        public DateTime? UPDDT { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? KodeUserGroup { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? KodeLokasi { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public string? UserInput { get; set; }
    }
}
