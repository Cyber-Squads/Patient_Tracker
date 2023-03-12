using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PatientTracker.Models
{
    [Table("PatientBillingDetails")]
    public class PatientBillingDL
    {
        [Key]
        public string PrescriptionID { get; set; }

        [DisplayName("Medicine Details")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="* Medicine Details")]
        public string MedicineDetails { get; set; }
        [DisplayName("Medicine Quantity")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "* Medicine Quantity")]
        public string MedicineQuantity { get; set; }
        [DisplayName("Other Billings ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "* Other Billings ")]
        public string OtherBillingsDetails { get; set; }
        [DisplayName("Billing Status ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "* Billing Status ")]
        public string BillingStatus { get; set; }
        [DisplayName("Billing Amount")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "* Billing Amount")]
        public int BillingAmount { get; set; }


        [DisplayName("Billing Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BillingDate { get; set; }
        public string PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual PatientDL patients { get; set; }

        public string DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual DoctorDL doctors { get; set; }

        public string TestId { get; set; }
        [ForeignKey("TestId")]
        public virtual MedicalTestDL MedicalTest { get; set; }

    }
}