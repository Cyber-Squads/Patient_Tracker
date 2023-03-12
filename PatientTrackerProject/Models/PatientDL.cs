using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PatientTracker.Models
{
    [Table("PatientDetails")]
    public class PatientDL
    {
        [Key]
        
        public string PatientId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "* First Name Required ")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "* Last Name Required ")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "* Age Required ")]


        public int Age { get; set; }
        [Required(ErrorMessage = "* Gender Required ")]
        [DisplayName("Gender")]
        public string Gender { get; set; }
        [RegularExpression(@"[9876]{1}[0-9]{9}", ErrorMessage = "Invalid Phone Number")]
        [Required(ErrorMessage = "* Contact Number Required ")]
        public string ContactNumber { get; set; }
        [Required(ErrorMessage = "* Password Required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{6,}$", ErrorMessage = "Min length of 6 with lowercase,uppercase and special characters required")]
        public string Password { get; set; }
        public string PatientStatus { get; set; }

        [DisplayName("Security Question 1")]
        [Required(ErrorMessage = "Please Select Security Question 1")]
        public string PatientSecuirtyQuestion1 { get; set; }
        [Required(ErrorMessage = "Please Answer the Security Question 1")]
        [DisplayName("Security Question 1 Answer")]
        public string PatientSecurityAnswer1 { get; set; }


        [DisplayName("Security Question 2")]
        [Required(ErrorMessage = "Please Select Security Question 2")]
        public string PatientSecuirtyQuestion2 { get; set; }
        [Required(ErrorMessage = "Please Answer the Security Question 2")]
        [DisplayName("Security Question 2 Answer")]
        public string PatientSecurityAnswer2 { get; set; }


        [DisplayName("Security Question 3")]
        [Required(ErrorMessage = "Please Select Security Question 3")]
        public string PatientSecuirtyQuestion3 { get; set; }
        [Required(ErrorMessage = "Please Answer the Security Question 3")]
        [DisplayName("Security Question 3 Answer")]
        public string PatientSecurityAnswer3 { get; set; }


        //public MedicalTestDL medicalTest { get; set; }
    }
}