using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagementSystem3.Models;

namespace ManagementSystem3.Controllers
{
    public class HomeController : Controller
    {
        ManagementSystemEntities dbobj = new ManagementSystemEntities();
        
        // GET: Home
        public ActionResult Index()
        {
            return View(dbobj.Registrations.ToList());
        }

        // SignUp
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(Registration registration)
        {
            if( dbobj.Registrations.Any(x => x.username == registration.username))
            {
                ViewBag.Notification = "Username already exists";
                return View();
                //new class userFunctions - separate dbobj - dependency injection
            }
            else
            {
                dbobj.Registrations.Add(registration);
                dbobj.SaveChanges();

                Session["idSS"] = registration.id.ToString();
                Session["usernameSS"] = registration.username.ToString();

                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Registration registration)
        {
            var VerifyLogin = dbobj.Registrations.Where(x => x.username.Equals(registration.username) && x.password.Equals(registration.password)).FirstOrDefault();
            if (VerifyLogin != null)
            {
                Session["idSS"] = registration.id.ToString();
                Session["usernameSS"] = registration.username.ToString();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Notification = "Wrong Username or Password";
            }
            return View();
        }
    }
}