using PatientTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PatientTracker.Controllers
{
   
    public class ClerkController : Controller
    {
        // GET: Clerk
        PatientTrackerContext trackerContext = new PatientTrackerContext();
        public ActionResult Index()
        {
            return View();
        }

       
        public ActionResult ClerkRegister()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClerkRegister(ClerkDL clerkDL)
        {
            try
            {

                Random rd = new Random();
                if (ModelState.IsValid)
                {
                    clerkDL.ClerkId = "CL" + rd.Next(1001, 9999).ToString();
                    clerkDL.ClerkStatus = "Pending";
                    trackerContext.Clerks.Add(clerkDL);
                    trackerContext.SaveChanges();
                    ViewBag.MessageSucess = "You Details are Successfully Submitted With Clerk ID  :" + clerkDL.ClerkId;


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

        public ActionResult ClerkLogin()
        {
            if (Session["ClerkId"] != null)
            {
                TempData.Keep();
                return RedirectToAction("ClerkHome");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClerkLogin(ClerkDL clerkDL)
        {
            try
            {
                if (clerkDL.ClerkId != null)
                {
                    var data = trackerContext.Clerks.Where(x => x.ClerkId == clerkDL.ClerkId && x.Password == clerkDL.Password).FirstOrDefault();
                    if (data != null && data.ClerkStatus == "Active")
                    {
                        Session["ClerkId"] = data.ClerkId.ToString();
                        TempData["User"] = data.FirstName;
                        TempData["UserId"] = data.ClerkId;
                        TempData.Keep();
                        return RedirectToAction("ClerkHome");
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


            return View(clerkDL);
        }

        public ActionResult ClerkHome()
        {
            if (Session["clerkId"] != null)
            {
                ViewBag.Welcome = TempData["User"];
                TempData.Keep();
                return View();
            }
            else
            {
                return RedirectToAction("ClerkLogin");
            }
        }

        public ActionResult ClerkLogout()
        {
            Response.Cookies.Clear();
            Session.Abandon();
            return RedirectToAction("ClerkLogin");
        }
        public ActionResult ClerkSearch()
        {
            return View();
        }

        public ActionResult ClerkPatientHome()
        {
            if (Session["ClerkId"] != null)
            {
                var data = trackerContext.Patients.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("ClerkLogin");
            }
            
        }

        public ActionResult EditClerkPatient(string id)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditClerkPatient(PatientDL patientDL)
        {
            if (ModelState.IsValid)
            {
                trackerContext.Entry(patientDL).State = System.Data.Entity.EntityState.Modified;
                trackerContext.SaveChanges();
                ViewBag.MessageSuccess = "Updated Details Successfully";
                return RedirectToAction("ClerkPatientHome");
            }
            return View();
        }

        public ActionResult ClerkPatientDetails(string id)
        {
            var data = trackerContext.Patients.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

        public ActionResult ClerkPatientDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientDL patientDL = trackerContext.Patients.Find(id);
            if (patientDL == null)
            {
                return HttpNotFound();
            }
            return View(patientDL);
        }
        [HttpPost, ActionName("ClerkPatientDelete")]
        public ActionResult DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                PatientDL patientDL = trackerContext.Patients.Find(id);
                trackerContext.Patients.Remove(patientDL);
                trackerContext.SaveChanges();
                return RedirectToAction("ClerkPatientHome");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed To Delete");
            }
            return View();
        }

        public ActionResult ClerkPatientTestDetails()
        {
            var data = trackerContext.medicalTests.ToList();
            return View(data);
        }

        public ActionResult EditClerkPatientTest(string id)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditClerkPatientTest(MedicalTestDL medicalTest)
        {
            try
            {

                trackerContext.Entry(medicalTest).State = EntityState.Modified;
                trackerContext.SaveChanges();
                ViewBag.Message = "data updated successfully";
                return RedirectToAction("ClerkPatientTestDetails");
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

        public ActionResult PatientTestDetails(string id)
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

        public ActionResult ClerkPatientTestDelete(string id)
        {
            if (Session["clerkId"] != null)
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
        [HttpPost, ActionName("ClerkPatientTestDelete")]
        public ActionResult ClerkPatientTestDeleteConfirmed(string id)
        {
            try
            {
                MedicalTestDL medicalTest = trackerContext.medicalTests.Find(id);
                trackerContext.medicalTests.Remove(medicalTest);
                trackerContext.SaveChanges();
                return RedirectToAction("ClerkPatientTestDetails");
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

        public ActionResult TestPrescriptionDetails(string id)
        {
            try
            {
                if (Session["clerkId"] != null)
                {
                    var data = trackerContext.patientBillings.Where(x => x.TestId == id);
                    int sum = 0;
                    foreach (PatientBillingDL i in data)
                    {
                        sum = sum + i.BillingAmount;
                    }
                    ViewBag.Amount = sum;
                    return View(data);
                }
                else
                {
                    return RedirectToAction("ClerkLogin");
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
        }
        public ActionResult EditTestPrescription(string id)
        {
            if (Session["clerkId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var data = trackerContext.patientBillings.Find(id);
                if (data == null)
                {
                    return HttpNotFound();
                }
                return View(data);
            }
            else
            {
                return RedirectToAction("ClerkLogin");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTestPrescription(PatientBillingDL patientBilling)
        {
            try
            {
                trackerContext.Entry(patientBilling).State = EntityState.Modified;
                trackerContext.SaveChanges();
                return RedirectToAction("ClerkPatientTestDetails");
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

        public ActionResult PrescriptionDetails(string id)
        {
            if (Session["clerkId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var data = trackerContext.patientBillings.Find(id);
                if (data == null)
                {
                    return HttpNotFound();
                }
                return View(data);
            }
            else
            {
                return RedirectToAction("ClerkLogin");
            }
        }

        public ActionResult TestPrescriptionDelete(string id)
        {
            if (Session["clerkId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var data = trackerContext.patientBillings.Find(id);
                if (data == null)
                {
                    return HttpNotFound();
                }
                return View(data);
            }
            else
            {
                return RedirectToAction("ClerkLogin");
            }
        }
        [HttpPost, ActionName("TestPrescriptionDelete")]
        public ActionResult TestPrescriptionDeleteConfirmed(string id)
        {
            try
            {
                PatientBillingDL patientBilling = trackerContext.patientBillings.Find(id);
                trackerContext.patientBillings.Remove(patientBilling);
                trackerContext.SaveChanges();
                return RedirectToAction("ClerkPatientTestDetails");
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

        public ActionResult ClerkDetails()
        {
            if (Session["clerkId"] != null)
            {
                
                string a = (string)TempData["UserId"];
                TempData.Keep();
                if (a == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                var data = trackerContext.Clerks.Find(a);
                if (data == null)
                {
                    return HttpNotFound();
                }
                return View(data);
            }
            else
            {
                return RedirectToAction("ClerkLogin");
            }

        }

        public ActionResult EditClerk(string id)
        {
            if (Session["clerkId"] != null)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                var data = trackerContext.Clerks.Find(id);
                if (data == null)
                {
                    return HttpNotFound();
                }
                return View(data);
            }
            else
            {
                return RedirectToAction("ClerkLogin");
            }
        }
        [HttpPost]
        public ActionResult EditClerk(ClerkDL clerkDL)
        {
            try
            {
                trackerContext.Entry(clerkDL).State = System.Data.Entity.EntityState.Modified;
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