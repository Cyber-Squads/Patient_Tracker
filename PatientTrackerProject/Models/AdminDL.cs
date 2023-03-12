using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PatientTracker.Models
{
    [Table("AdminDetails")]
    public class AdminDL
    {
        [DisplayName("Admin ID")]
        [Key]
        [Required(ErrorMessage = "* Admin's User Id Required")]
        public string AdminId { get; set; }
        [DisplayName("Admin Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "* Admin's User Password Required")]
        public string AdminPassword { get; set; }
    }
}