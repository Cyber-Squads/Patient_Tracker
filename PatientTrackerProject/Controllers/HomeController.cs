using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PatientTracker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult TrackingHome()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TrackingHome(string Role)
        {
            if (Role == "Admin")
            {
                return RedirectToAction("AdminLogin","Admin");
            }
            else if (Role == "Doctor")
            {
                return RedirectToAction("DoctorLogin","Doctor");
            }
            else if (Role == "Patient")
            {
                return RedirectToAction("PatientLogin","Patient");
            }
            else if (Role == "Clerk")
            {
                return RedirectToAction("ClerkLogin","Clerk");
            }
            else
            {
                ViewBag.Message = "Select Your Role";
            }
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }
    }
}