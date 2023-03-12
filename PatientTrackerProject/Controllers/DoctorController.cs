using PatientTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PatientTracker.Controllers
{
    public class DoctorController : Controller
    {
        // GET: Doctor
        PatientTrackerContext trackerContext = new PatientTrackerContext();
        public ActionResult Index()
        {
            return View();
        }

        

        public ActionResult DoctorRegister()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoctorRegister(DoctorDL doctorDL)
        {
            try
            {
                
                Random rd = new Random();
                if (ModelState.IsValid)
                {
                    doctorDL.DoctorId = "DO" + rd.Next(1001, 9999).ToString();
                    doctorDL.DoctorStatus = "Pending";
                    trackerContext.Doctors.Add(doctorDL);
                    trackerContext.SaveChanges();
                    ViewBag.MessageSucess = "You Details are Successfully Submitted With Doctor ID  :" + doctorDL.DoctorId;
                    

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Unable to Register");
                }
            }
            catch
            {
                string controllername = this.ControllerContext.RouteData.Values["controller"].ToString();
                string actionname = this.ControllerContext.RouteData.Values["action"].ToString();
                string message = "Exception occured in " + actionname + " in " + controllername;

                TempData["errorMessage"] = message;
                return RedirectToAction("ErrorInfo");
            }
            return View();
        }
        public ActionResult ErrorInfo()
        {
            ViewBag.Message = TempData["errorMessage"];
            return View();
        }
        public ActionResult DoctorLogin()
        {
            if (Session["DoctorId"] != null)
            {
                return RedirectToAction("DoctorHome");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoctorLogin(DoctorDL doctorDL)
        {
            try
            {
                if (doctorDL.DoctorId != null)
                {
                    PatientTrackerContext patientContext1 = new PatientTrackerContext();
                    var sad = patientContext1.Doctors.Where(x => x.DoctorId == doctorDL.DoctorId && x.Password == doctorDL.Password).FirstOrDefault();
                    if (sad != null && sad.DoctorStatus == "Active")
                    {
                        Session["DoctorId"] = sad.DoctorId.ToString();
                        TempData["User"] = sad.FirstName;
                        TempData["UserId"] = sad.DoctorId;
                        TempData.Keep();

                        return RedirectToAction("DoctorHome");
                    }
                    else
                    {
                        ViewBag.Message = "Please Register or Your Status is Pending Wait for Approve";
                    }

                }
                else
                {
                    ViewBag.MessageLO = "* Doctor Id Required";
                }
            }
            catch
            {
                string controllername = this.ControllerContext.RouteData.Values["controller"].ToString();
                string actionname = this.ControllerContext.RouteData.Values["action"].ToString();
                string message = "Exception occured in " + actionname + " in " + controllername;

                TempData["errorMessage"] = message;
                return RedirectToAction("ErrorInfo");
            }
            return View(doctorDL);
        }

        public ActionResult DoctorHome()
        {
            if (Session["DoctorId"] != null)
            {
                ViewBag.Welcome = TempData["User"];
                TempData.Keep();
                return View();
            }
            else
            {
                return RedirectToAction("DoctorLogin");
            }
        }
        public ActionResult DoctorLogout()
        {
            Response.Cookies.Clear();
            Session.Abandon();
            return RedirectToAction("DoctorLogin");
        }

        public ActionResult DoctorSearch()
        {
            return View();
        }

        public ActionResult DoctorPatientInfo()
        {
            if (Session["DoctorId"] != null)
            {
                
                string a = (string)TempData["UserId"];
                ViewBag.Message = a;
                var data = trackerContext.medicalTests.Where(x => x.DoctorId == a).ToList();
                //foreach(MedicalTestDL i  in data)
                //{
                //    TempData["TestId"] = i.TestId;
                //}
                TempData.Keep();
               
                return View(data);
            }
            else
            {
                return RedirectToAction("DoctorLogin");
            }
            
        }

        public ActionResult EditDoctorPatientInfo(string id)
        {
            if (Session["DoctorId"] != null)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var data = trackerContext.medicalTests.Find(id);
                if (data == null)
                {
                    return HttpNotFound();
                }
                return View(data);
            }
            else
            {
                return RedirectToAction("DoctorLogin");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDoctorPatientInfo(MedicalTestDL medicalTest)
        {
            try
            {
                trackerContext.Entry(medicalTest).State = System.Data.Entity.EntityState.Modified;
                trackerContext.SaveChanges();

                return RedirectToAction("DoctorPatientInfo");
            }
            catch
            {
                string controllername = this.ControllerContext.RouteData.Values["controller"].ToString();
                string actionname = this.ControllerContext.RouteData.Values["action"].ToString();
                string message = "Exception occured in " + actionname + " in " + controllername;

                TempData["errorMessage"] = message;
                return RedirectToAction("ErrorInfo");
            }
        }

        public ActionResult DoctorPatientDetails(string id)
        {
            if (Session["DoctorId"] != null)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var data = trackerContext.medicalTests.Find(id);
                if (data == null)
                {
                    return HttpNotFound();
                }
                return View(data);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }

        public ActionResult DeleteDoctorPatient(string id)
        {
            if (Session["DoctorId"] != null)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var data = trackerContext.medicalTests.Find(id);
                if (data == null)
                {
                    return HttpNotFound();
                }
                return View(data);
            }
            else
            {
                return RedirectToAction("DoctorLogin");
            }
        }
        [HttpPost,ActionName("DeleteDoctorPatient")]
        public ActionResult DeleteDoctorPatientConfirmed(string id)
        {
            try
            {
                MedicalTestDL data = trackerContext.medicalTests.Find(id);
                trackerContext.medicalTests.Remove(data);
                trackerContext.SaveChanges();
                return RedirectToAction("DoctorPatientInfo");
            }
            catch
            {
                string controllername = this.ControllerContext.RouteData.Values["controller"].ToString();
                string actionname = this.ControllerContext.RouteData.Values["action"].ToString();
                string message = "Exception occured in " + actionname + " in " + controllername;

                TempData["errorMessage"] = message;
                return RedirectToAction("ErrorInfo");
            }
        }

        public ActionResult DoctorPatientPersonalInfo(string id)
        {
            if (Session["DoctorId"] != null)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var data = trackerContext.Patients.Find(id);

                if (data == null)
                {
                    return HttpNotFound();
                }
                return View(data);
            }
            else
            {
                return RedirectToAction("DoctorLogin");
            }
        }

        
        public ActionResult PatientPrecription()
        {
            
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        
        public ActionResult PatientPrecription(string id, string pi, PatientBillingDL patientBilling)
        {
            try
            {
                Random rd = new Random();

                patientBilling.PrescriptionID = "PR" + rd.Next(1000, 9999).ToString();
                string a = (string)TempData["UserId"];
                TempData.Keep();
                patientBilling.DoctorId = a;
                patientBilling.PatientId = pi;
                patientBilling.TestId = id;
                patientBilling.BillingStatus = "Pending";
                patientBilling.BillingAmount = 0000;
                patientBilling.OtherBillingsDetails = "NA";
                trackerContext.patientBillings.Add(patientBilling);
                trackerContext.SaveChanges();
                ViewBag.Message = "Prescription Generated with PrescriptionId :" + patientBilling.PrescriptionID;


                return View();
            }
            catch
            {
                string controllername = this.ControllerContext.RouteData.Values["controller"].ToString();
                string actionname = this.ControllerContext.RouteData.Values["action"].ToString();
                string message = "Exception occured in " + actionname + " in " + controllername;

                TempData["errorMessage"] = message;
                return RedirectToAction("ErrorInfo");
            }
        }

        public ActionResult DoctorDetails()
        {
            if (Session["DoctorId"] != null)
            {


                string a = (string)TempData["UserId"];
                TempData.Keep();
                if (a == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                var data = trackerContext.Doctors.Find(a);
                if (data == null)
                {
                    return HttpNotFound();
                }
                return View(data);
            }
            else
            {
                return RedirectToAction("DoctorId");
            }
        }
        
        public ActionResult EditDoctor(string id)
        {
            if (Session["DoctorId"] != null)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                var data = trackerContext.Doctors.Find(id);
                if (data == null)
                {
                    return HttpNotFound();
                }
                return View(data);
            }
            else
            {
                return RedirectToAction("PatientLogin");
            }
        }
        [HttpPost]
        public ActionResult EditDoctor(DoctorDL doctor)
        {
            try
            {
                trackerContext.Entry(doctor).State = System.Data.Entity.EntityState.Modified;
                trackerContext.SaveChanges();
                ViewBag.Message = "Updated Successfully";
                return View();
            }
            catch
            {
                string controllername = this.ControllerContext.RouteData.Values["controller"].ToString();
                string actionname = this.ControllerContext.RouteData.Values["action"].ToString();
                string message = "Exception occured in " + actionname + " in " + controllername;

                TempData["errorMessage"] = message;
                return RedirectToAction("ErrorInfo");
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                trackerContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}