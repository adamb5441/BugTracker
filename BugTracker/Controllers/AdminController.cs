using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.User = new SelectList(db.Users, "Id", "Email");

            ViewBag.Roles = new SelectList(db.Roles, "Name", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string User, string Roles)
        {
            return RedirectToAction("Index", "Home");
        }
    }

}