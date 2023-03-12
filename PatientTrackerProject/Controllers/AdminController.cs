using PatientTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;

namespace PatientTracker.Controllers
{
    public class AdminController : Controller
    {
        // GET: Management
        PatientTrackerContext trackerContext = new PatientTrackerContext();
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult AdminLogin()
        {
            if (Session["Admin1"] != null)
            {


                return RedirectToAction("AdminDoctorHome");
            }
            else if (Session["Admin2"] != null)
            {


                return RedirectToAction("AdminPatientHome");
            }
            else if (Session["Admin3"] != null)
            {


                return RedirectToAction("AdminClerkHome");
            }
            else if (Session["Admin4"] != null)
            {


                return RedirectToAction("AdminUserInfo");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(AdminDL adminDL)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = trackerContext.Admins.Where(x => x.AdminId == adminDL.AdminId && x.AdminPassword == adminDL.AdminPassword).FirstOrDefault();
                    if (data != null)
                    {
                        if (data.AdminId == "Admin1")
                        {
                            Session["Admin1"] = data.AdminId;
                            TempData["User"] = data.AdminId;
                            TempData.Keep();
                            return RedirectToAction("AdminDoctorHome");
                        }
                        else if (data.AdminId == "Admin2")
                        {
                            Session["Admin2"] = data.AdminId;
                            TempData["User"] = data.AdminId;
                            TempData.Keep();
                            return RedirectToAction("AdminPatientHome");
                        }
                        else if (data.AdminId == "Admin3")
                        {
                            Session["Admin3"] = data.AdminId;
                            TempData["User"] = data.AdminId;
                            TempData.Keep();
                            return RedirectToAction("AdminClerkHome");
                        }
                        else if (data.AdminId == "Admin4")
                        {
                            Session["Admin4"] = data.AdminId;
                            TempData["User"] = data.AdminId;
                            TempData.Keep();
                            return RedirectToAction("AdminUserInfo");
                        }
                        else
                        {
                            ViewBag.Message = "AdminId and Password are Incorrect";
                        }
                    }
                    else
                    {
                        ViewBag.MessageX = "AdminId and Password are not Valid";
                    }



                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Unable to Login");
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
        public ActionResult AdminUserInfo()
        {
            if (Session["Admin4"] != null)
            {
                TempData.Keep();
                ViewBag.Welcome = TempData["User"];
                return View();
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }

        }

        public ActionResult AdminDoctorHome()
        {
            if (Session["Admin1"] != null)
            {
                ViewBag.Welcome = TempData["User"];
                TempData.Keep();
                var DoctorList = trackerContext.Doctors.ToList();
                return View(DoctorList);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }



        }


        public ActionResult AdminClerkHome()
        {
            if (Session["Admin3"] != null)
            {
                TempData.Keep();
                ViewBag.Welcome = TempData["User"];
                var ClerkList = trackerContext.Clerks.ToList();
                return View(ClerkList);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }

        }


        public ActionResult AdminPatientHome()
        {
            if (Session["Admin2"] != null)
            {
                TempData.Keep();
                ViewBag.Welcome = TempData["User"];
                var PatientList = trackerContext.Patients.ToList();
                return View(PatientList);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }


        }

        public ActionResult AdminDoctorHomeLogout()
        {
            Response.Cookies.Clear();
            Session.Abandon();
            return RedirectToAction("AdminLogin");
        }
        public ActionResult AdminPatientHomeLogout()
        {
            Response.Cookies.Clear();
            Session.Abandon();
            return RedirectToAction("AdminLogin");
        }
        public ActionResult AdminClerkHomeLogout()
        {
            Response.Cookies.Clear();
            Session.Abandon();
            return RedirectToAction("AdminLogin");
        }
        public ActionResult AdminUserinfoLogout()
        {
            Response.Cookies.Clear();
            Session.Abandon();
            
            return RedirectToAction("AdminLogin");
        }
        //Patient--------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult EditPatient(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientDL patient = trackerContext.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        [HttpPost]
        public ActionResult EditPatient(PatientDL patientDL)
        {
            if (ModelState.IsValid)
            {
                trackerContext.Entry(patientDL).State = EntityState.Modified;
                trackerContext.SaveChanges();
                ViewBag.Message = "Updated Successfully";
                return RedirectToAction("AdminPatientInfo");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to Edit");
            }
            return View();
        }

        public ActionResult PatientDetails(string id)
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

        public ActionResult DeletePatient(string id)
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
        [HttpPost, ActionName("DeletePatient")]
        public ActionResult DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                PatientDL patientDL = trackerContext.Patients.Find(id);
                trackerContext.Patients.Remove(patientDL);
                trackerContext.SaveChanges();
                return RedirectToAction("AdminPatientInfo");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed To Delete");
            }
            return View();
        }
        //Patient-------------------------------------------------------------------------------------
        //Doctor--------------------------------------------------------------------------------------
        public ActionResult EditDoctor(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorDL doctorDL = trackerContext.Doctors.Find(id);
            if (doctorDL == null)
            {
                return HttpNotFound();
            }
            return View(doctorDL);
        }
        [HttpPost]
        public ActionResult EditDoctor(DoctorDL doctorDL)
        {
            if (ModelState.IsValid)
            {
                trackerContext.Entry(doctorDL).State = EntityState.Modified;
                trackerContext.SaveChanges();
                ViewBag.Message = "Updated Successfully";
                return RedirectToAction("AdminDoctorInfo");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed To Edit");
            }
            return View();
        }

        public ActionResult DoctorDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorDL doctorDL = trackerContext.Doctors.Find(id);
            if (doctorDL == null)
            {
                return HttpNotFound();
            }
            return View(doctorDL);
        }
        public ActionResult DeleteDoctor(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorDL doctorDL = trackerContext.Doctors.Find(id);
            if (doctorDL == null)
            {
                return HttpNotFound();
            }
            return View(doctorDL);


        }
        [HttpPost, ActionName("DeleteDoctor")]
        public ActionResult DoctorDeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                DoctorDL doctorDL = trackerContext.Doctors.Find(id);
                trackerContext.Doctors.Remove(doctorDL);
                trackerContext.SaveChanges();
                return RedirectToAction("AdminDoctorInfo");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed To Delete");
            }

            return View();
        }
        //Doctor--------------------------------------------------------------------------------------------
        //Clerk --------------------------------------------------------------------------------------------

        public ActionResult EditClerk(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClerkDL clerkDL = trackerContext.Clerks.Find(id);
            if (clerkDL == null)
            {
                return HttpNotFound();
            }
            return View(clerkDL);
        }

        [HttpPost]
        public ActionResult EditClerk(ClerkDL clerkDL)
        {
            if (ModelState.IsValid)
            {
                trackerContext.Entry(clerkDL).State = EntityState.Modified;
                trackerContext.SaveChanges();
                ViewBag.Message = "Updated Successfully";
                return RedirectToAction("AdminClerkInfo");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed To Edit");
            }
            return View();
        }

        public ActionResult ClerkDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClerkDL clerkDL = trackerContext.Clerks.Find(id);
            if (clerkDL == null)
            {
                return HttpNotFound();
            }
            return View(clerkDL);
        }
        public ActionResult DeleteClerk(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClerkDL clerkDL = trackerContext.Clerks.Find(id);
            if (clerkDL == null)
            {
                return HttpNotFound();
            }
            return View(clerkDL);


        }
        [HttpPost, ActionName("DeleteClerk")]
        public ActionResult ClerkDeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                ClerkDL clerkDL = trackerContext.Clerks.Find(id);
                trackerContext.Clerks.Remove(clerkDL);
                trackerContext.SaveChanges();
                return RedirectToAction("AdminClerkInfo");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed To Delete");
            }
            return View();
        }

        //------------------------------------------------------------------------------

        public ActionResult AdminPatientInfo()
        {
            if (Session["Admin4"] != null)
            {
                ViewBag.Welcome = TempData["User"];
                TempData.Keep();
                var data = trackerContext.Patients.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }
        [HttpPost]
        public ActionResult AdminPatientInfo(string Search)
        {
            if (Session["Admin4"] != null)
            {
                ViewBag.Welcome = TempData["User"];
                TempData.Keep();
                var data = trackerContext.Patients.Where(x=>x.FirstName.StartsWith(Search)).ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }
        public ActionResult AdminDoctorInfo()
        {
            if (Session["Admin4"] != null)
            {
                ViewBag.Welcome = TempData["User"];
                TempData.Keep();
                var data = trackerContext.Doctors.ToList();
                return View(data);

            }
            else
            {
                return RedirectToAction("AdminLogin");
            }

        }
        [HttpPost]
        public ActionResult AdminDoctorInfo(string Search)
        {
            if (Session["Admin4"] != null)
            {
                ViewBag.Welcome = TempData["User"];
                TempData.Keep();
                var data = trackerContext.Doctors.Where(x => x.FirstName.StartsWith(Search)).ToList();
                return View(data);

            }
            else
            {
                return RedirectToAction("AdminLogin");
            }

        }
        public ActionResult AdminClerkInfo()
        {
            if (Session["Admin4"] != null)
            {
                
                ViewBag.Welcome = TempData["User"];
                TempData.Keep();
                var data = trackerContext.Clerks.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
            
        }

        public ActionResult AdminPatientTestInfo()
        {
            if (Session["Admin4"] != null)
            {
                ViewBag.Welcome = TempData["User"];
                TempData.Keep();
                var data = trackerContext.medicalTests.ToList();

                return View(data);
            }
            return RedirectToAction("AdminLogin");
            
        }

        public ActionResult EditPatientTest(string id)
        {
            if (Session["Admin4"] != null)
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
        [HttpPost]
        public ActionResult EditPatientTest(MedicalTestDL medicalTest)
        {
            try
            {

                trackerContext.Entry(medicalTest).State = EntityState.Modified;
                trackerContext.SaveChanges();
                ViewBag.Message = "data updated successfully";
                return RedirectToAction("AdminPatientTestInfo");
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
            if (Session["Admin4"] != null)
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

        public ActionResult PatientTestDelete(string id)
        {
            if (Session["Admin4"] != null)
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
        [HttpPost,ActionName("PatientTestDelete")]
        public ActionResult PatientTestDeleteConfirmed(string id)
        {
            try
            {
                MedicalTestDL medicalTest = trackerContext.medicalTests.Find(id);
                trackerContext.medicalTests.Remove(medicalTest);
                trackerContext.SaveChanges();
                return RedirectToAction("AdminPatientTestInfo");
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
                if (Session["Admin4"] != null)
                {
                    var data = trackerContext.patientBillings.Where(x => x.TestId == id);
                    int sum = 0;
                    foreach(PatientBillingDL i in data)
                    {
                        sum = sum + i.BillingAmount;
                    }
                    ViewBag.Amount = sum;
                    return View(data);
                }
                else
                {
                    return RedirectToAction("AdminLogin");
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
            if (Session["Admin4"] != null)
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
                return RedirectToAction("AdminLogin");
            }
        }
        [HttpPost]
        public ActionResult EditTestPrescription(PatientBillingDL patientBilling)
        {
            try
            {
                trackerContext.Entry(patientBilling).State = EntityState.Modified;
                trackerContext.SaveChanges();
                return RedirectToAction("AdminPatientTestInfo");
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
            if (Session["Admin4"] != null)
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
                return RedirectToAction("AdminLogin");
            }
        }

        public ActionResult TestPrescriptionDelete(string id)
        {
            if (Session["Admin4"] != null)
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
                return RedirectToAction("AdminLogin");
            }
        }
        [HttpPost,ActionName("TestPrescriptionDelete")]
        public ActionResult TestPrescriptionDeleteConfirmed(string id)
        {
            try
            {
                PatientBillingDL patientBilling = trackerContext.patientBillings.Find(id);
                trackerContext.patientBillings.Remove(patientBilling);
                trackerContext.SaveChanges();
                return RedirectToAction("AdminPatientTestInfo");
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