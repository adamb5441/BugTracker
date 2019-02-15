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
using Microsoft.AspNet.Identity;
namespace BugTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper rolerHelpers = new UserRoleHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        
        // GET: Projects
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Project Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Project project)
        {

            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                if (User.IsInRole("Project Manager"))
                {
                    projectHelper.AddUserToProject(User.Identity.GetUserId(),project.Id);
                }
                return RedirectToAction("Index", "Home");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Edit(int? id)
        {
            var userId = User.Identity.GetUserId();
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            var users = projectHelper.UsersOnProject(id ?? 0);
            var pmId = "";
            var subId = "";
            var adminId = "";
            List<string> devIds = new List<string>();

            foreach (var user in users)
            {
                if(rolerHelpers.IsUserInRole(user.Id, "Project Manager"))
                {
                    pmId = user.Id;
                }
                if (rolerHelpers.IsUserInRole(user.Id, "Submitter"))
                {
                    subId = user.Id;
                }
                if (rolerHelpers.IsUserInRole(user.Id, "Developer"))
                {
                    devIds.Add(user.Id);
                }
                if (rolerHelpers.IsUserInRole(user.Id, "Admin"))
                {
                    adminId = userId;
                }
            }


            var admin = rolerHelpers.UsersInRole("Admin");
            ViewBag.Admin = new SelectList(admin, "Id", "Email", adminId);

            var pms = rolerHelpers.UsersInRole("Project Manager");
            ViewBag.ProjectManager = new SelectList(pms, "Id", "Email", pmId);

            var sub = rolerHelpers.UsersInRole("Submitter");
            ViewBag.Submitter = new SelectList(sub, "Id", "Email",subId);

            var dv = rolerHelpers.UsersInRole("Developer");
            ViewBag.Developer = new MultiSelectList(dv, "Id", "Email", devIds);

            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Project Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Project project, List<string> Developer, string Submitter, string ProjectManager, string Admin)
        {
            if (ModelState.IsValid)
            {
                var users = projectHelper.UsersOnProject(project.Id).ToList();
                foreach (var use in users)
                {
                    projectHelper.RemoveUserFromProject(use.Id, project.Id);
                }
                if (Developer != null)
                    foreach (var dev in Developer)
                    {
                        projectHelper.AddUserToProject(dev, project.Id);
                    }

                if (Submitter != null)
                    projectHelper.AddUserToProject(Submitter, project.Id);

                if (ProjectManager != null)
                    projectHelper.AddUserToProject(ProjectManager, project.Id);

                if (Admin != null)
                    projectHelper.AddUserToProject(Admin, project.Id);

                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
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
