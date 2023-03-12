using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PatientTracker.Models
{
    public class PatientTrackerContext:DbContext
    {
        public PatientTrackerContext() : base("Name=connect")
        {

        }
        public virtual DbSet<AdminDL> Admins { get; set; }

        public System.Data.Entity.DbSet<PatientTracker.Models.DoctorDL> Doctors { get; set; }

        public System.Data.Entity.DbSet<PatientTracker.Models.PatientDL> Patients { get; set; }

        public System.Data.Entity.DbSet<PatientTracker.Models.ClerkDL> Clerks { get; set; }

       public System.Data.Entity.DbSet<PatientTracker.Models.MedicalTestDL> medicalTests { get; set; }

        public virtual DbSet<PatientBillingDL> patientBillings { get; set; }
    }
}