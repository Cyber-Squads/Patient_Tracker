using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PatientTracker.Models
{
    [Table("MedicalTestDetails")]
    public class MedicalTestDL
    {
        [Key]
        public string TestId { get; set; }
        
        [DisplayName("Test Type")]
        [Required(ErrorMessage ="* Test Type required")]
        public string TestType { get; set; }
        [DisplayName("Type of Doctor")]
        [Required(ErrorMessage = "* Doctor Type required")]
        public string DoctorType { get; set; }
        [DisplayName("Test Appointment Date ")]
        [Required(ErrorMessage = "* Test Appointment Date required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfTestAppointment { get; set; }
        [DisplayName("Raise Test Request")]
        [Required(ErrorMessage = "* Raise Test  required")]

        public string TestRequest { get; set; }

        [DisplayName("Test Completed Date")]
        [Required(ErrorMessage = "* Test Completed Date required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TestCompletedDate { get; set; }
        
        [DisplayName("Test Result")]
        [Required(ErrorMessage = "* Test Result required")]

        public string TestResult { get; set; }
        [Required(ErrorMessage ="* Test Status required")]
        public string TestStatus { get; set; }
        [DisplayName("Test Description")]
        [Required(ErrorMessage = "* Test Description required")]
        public string TestDescription { get; set; }

        [DisplayName("Treatment")]
        [Required(ErrorMessage = "* Treatment required")]
        public string Treatment { get; set; }

        
        public string PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual PatientDL patients { get; set; }
        
        public string DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual DoctorDL doctors { get; set; }
    }
}