using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PatientTracker.Models
{
    [Table("DoctorDetails")]
    public class DoctorDL
    {
        [Key]
        
        public string DoctorId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "* First Name Required ")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "* Last Name Required ")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "* DOB Required ")]
        [DisplayName("DOB")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }
        [Required(ErrorMessage = "* Gender Required ")]
        [DisplayName("Gender")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "* Contact Number Required ")]
        [RegularExpression(@"[9876]{1}[0-9]{9}", ErrorMessage = "Invalid Phone Number")]
        public string ContactNumber { get; set; }
        [Required(ErrorMessage = "* Password Required")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{6,}$", ErrorMessage = "Min length of 6 with lowercase,uppercase and special characters required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "* Doctor Qualification Required")]
        public string DoctorQualification { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = " * Address Required")]
        public string Address { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "* Speciality Required")]
        public string Speciality { get; set; }

        public string DoctorStatus { get; set; }

        [DisplayName("Security Question 1")]
        [Required(ErrorMessage = "Please Select Security Question 1")]
        public string DoctorSecuirtyQuestion1 { get; set; }
        [Required(ErrorMessage = "Please Answer the Security Question 1")]
        [DisplayName("Security Question 1 Answer")]
        public string DoctorSecurityAnswer1 { get; set; }

        [DisplayName("Security Question 2")]
        [Required(ErrorMessage = "Please Select Security Question 2")]
        public string DoctorSecuirtyQuestion2 { get; set; }
        [Required(ErrorMessage = "Please Answer the Security Question 2")]
        [DisplayName("Security Question 2 Answer")]
        public string DoctorSecurityAnswer2 { get; set; }


        [DisplayName("Security Question 3")]
        [Required(ErrorMessage = "Please Select Security Question 3")]
        public string DoctorSecuirtyQuestion3 { get; set; }
        [Required(ErrorMessage = "Please Answer the Security Question 3")]
        [DisplayName("Security Question 3 Answer")]
        public string DoctorSecurityAnswer3 { get; set; }

       // public MedicalTestDL medicalTest { get; set; }

    }
}