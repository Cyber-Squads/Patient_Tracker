using PatientTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PatientTracker.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient
        PatientTrackerContext trackerContext = new PatientTrackerContext();
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult PatientRegister()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult PatientRegister(PatientDL patientDL)
        {
            try
            {
                Random rd = new Random();
                if (ModelState.IsValid)
                {
                    patientDL.PatientId = "PA" + rd.Next(1001, 9999).ToString();
                    patientDL.PatientStatus = "Pending";
                    trackerContext.Patients.Add(patientDL);
                    trackerContext.SaveChanges();
                    ViewBag.MessageSucess = "You Details are Successfully Submitted With Patient ID :" + patientDL.PatientId;

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
        public ActionResult PatientLogin()
        {
            if (Session["PatientId"] != null)
            {
                return RedirectToAction("PatientHome");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PatientLogin(PatientDL patientDL)
        {
            try
            {
                if (patientDL.PatientId != null)
                {
                    var data = trackerContext.Patients.Where(x => x.PatientId == patientDL.PatientId && x.Password == patientDL.Password).FirstOrDefault();

                    if (data != null && data.PatientStatus == "Active")
                    {
                        Session["PatientId"] = data.PatientId.ToString();
                        TempData["User"] = data.FirstName;
                        TempData["UserId"] = data.PatientId;
                        TempData.Keep();
                        return RedirectToAction("PatientHome");
                    }
                    else
                    {
                        ViewBag.Message = "Please Register or Your Status is Pending Wait for Approve";
                    }
                }
                else
                {
                    ViewBag.MessageLO = "* Patient Id Required";
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
            return View(patientDL);
        }
        public ActionResult ErrorInfo()
        {
            ViewBag.Message = TempData["errorMessage"];
            return View();
        }
        public ActionResult PatientHome()
        {
            if (Session["PatientId"] != null)
            {
                ViewBag.Welcome = TempData["User"];
                TempData.Keep();
                return View();
            }
            else
            {
                return RedirectToAction("PatientLogin");
            }
            
        }

        public ActionResult PatientLogout()
        {
            Response.Cookies.Clear();
            Session.Abandon();

            return RedirectToAction("PatientLogin");
        }
       
        public ActionResult PatientSearch()
        {
            return View();
        }
       

        public ActionResult PatientRaiseTest()
        {
            if (Session["PatientId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("PatientLogin");
            }
           
            
        }
        [HttpPost]
        public ActionResult PatientRaiseTest(MedicalTestDL medicalTest)
        {
            try
            {

                Random rd = new Random();
                var data = trackerContext.Doctors.Where(x => x.Speciality == medicalTest.DoctorType).ToList();
                List<String> li = new List<String>();
                foreach(DoctorDL i in data)
                {

                    li.Add(i.DoctorId);
                }
                
                medicalTest.TestId = "TE" + rd.Next(1000, 9999).ToString();

                int res = rd.Next(li.Count);


                medicalTest.DoctorId = li[res];
                medicalTest.PatientId = (string)TempData["UserId"];
                medicalTest.TestResult = "Pending";
                
                medicalTest.Treatment = "NA";
                medicalTest.TestCompletedDate = medicalTest.DateOfTestAppointment;
                medicalTest.TestStatus = "Pending";
                string s = medicalTest.TestId + "" + medicalTest.TestType + "" + medicalTest.TestRequest + "" + medicalTest.TestResult + "" + medicalTest.TestDescription + "" + medicalTest.Treatment + "" + medicalTest.PatientId + "" + medicalTest.DoctorId + "" + medicalTest.DateOfTestAppointment + "" + medicalTest.TestCompletedDate + "" + medicalTest.TestStatus;
                trackerContext.medicalTests.Add(medicalTest);
                trackerContext.SaveChanges();

                ViewBag.Message = "You have Raised Test Appointment successfully with Doctor Id :" + medicalTest.DoctorId +" and Test Id "+medicalTest.TestId;
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

        public ActionResult PatientDetails()
        {
            if (Session["PatientId"] != null)
            {
                string a = (string)TempData["UserId"];
                TempData.Keep();
                if (a == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                var data = trackerContext.Patients.Find(a);
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

        public ActionResult EditPatient(string id)
        {
            if (Session["PatientId"] != null)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
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
                return RedirectToAction("PatientLogin");
            }
        }
        [HttpPost]
        public ActionResult EditPatient(PatientDL patient)
        {
            try
            {
                trackerContext.Entry(patient).State = System.Data.Entity.EntityState.Modified;
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

        public ActionResult PatientTestDetails()
        {
            if (Session["PatientId"] != null)
            {
                string a = (string)TempData["UserId"];
                TempData.Keep();
                var data = trackerContext.medicalTests.Where(x => x.PatientId == a && x.TestStatus=="Completed").ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("PatientLogin");
            }
        }

        public ActionResult PatientMedicineDetails()
        {
            if (Session["PatientId"] != null)
            {
                string a = (string)TempData["UserId"];
                TempData.Keep();
                var data = trackerContext.patientBillings.Where(x => x.PatientId == a && x.BillingStatus=="Billed").ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("PatientLogin");
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