using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helpers;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper rolesHelper = new UserRoleHelper();

        // GET: Users
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: Users/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,DisplayName,AvatarUrl,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser, string roles)
        {
            if (ModelState.IsValid)
            {

                db.Users.Add(applicationUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();

            }
            var currentrole = rolesHelper.ListUserRoles(id).FirstOrDefault();

            ViewBag.Roles = new SelectList(db.Roles, "Name", "Name", currentrole);
            return View(applicationUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,PhoneNumber")] ApplicationUser applicationUser, string roles)
        {
            if (ModelState.IsValid)
            {

                var currentrole = rolesHelper.ListUserRoles(applicationUser.Id);
                if (User.IsInRole("Admin"))
                {
                    foreach (var role in currentrole)
                    {
                        rolesHelper.RemoveUserFromRole(applicationUser.Id, role);
                    }
                    if (!string.IsNullOrEmpty(roles))
                        rolesHelper.AddUsertoRole(applicationUser.Id, roles);
                }
                applicationUser.UserName = applicationUser.Email;
                db.Users.Attach(applicationUser);
                db.Entry(applicationUser).Property(x => x.FirstName).IsModified = true;
                db.Entry(applicationUser).Property(x => x.LastName).IsModified = true;
                db.Entry(applicationUser).Property(x => x.Email).IsModified = true;
                db.Entry(applicationUser).Property(x => x.DisplayName).IsModified = true;
                db.Entry(applicationUser).Property(x => x.UserName).IsModified = true;
                db.Entry(applicationUser).Property(x => x.PhoneNumber).IsModified = true;

                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(applicationUser);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
