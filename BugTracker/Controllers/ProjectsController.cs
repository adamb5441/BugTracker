﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        private ArchiveHelper archiveHelper = new ArchiveHelper();

        // GET: Projects/Details/5
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Details(int id)
        {
            var isOnProj = projectHelper.IsUserOnProject(User.Identity.GetUserId(), id);
            if (User.IsInRole("Project Manager")&& !isOnProj)
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
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Archive()
        {
            ViewBag.Archive = false;
            var projects = archiveHelper.GetArchivedProjects();
            return View(projects);
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
            var adminId = "";
            var sub = "";
            List<string> devIds = new List<string>();

            foreach (var user in users)
            {
                if(rolerHelpers.IsUserInRole(user.Id, "Project Manager"))
                {
                    pmId = user.Id;
                }

                if (rolerHelpers.IsUserInRole(user.Id, "Developer"))
                {
                    devIds.Add(user.Id);
                }
                if (rolerHelpers.IsUserInRole(user.Id, "Admin"))
                {
                    adminId = user.Id;
                }
                if (rolerHelpers.IsUserInRole(user.Id, "Submitter"))
                {
                    sub = user.Id;
                }
            }


            var Submitter = rolerHelpers.UsersInRole("Submitter");
            ViewBag.Submitter = new SelectList(Submitter, "Id", "Email", sub);

            var admin = rolerHelpers.UsersInRole("Admin");
            ViewBag.Admin = new SelectList(admin, "Id", "Email", adminId);

            var pms = rolerHelpers.UsersInRole("Project Manager");
            ViewBag.ProjectManager = new SelectList(pms, "Id", "Email", pmId);

            var dv = rolerHelpers.UsersInRole("Developer");
            ViewBag.Developer = new MultiSelectList(dv, "Id", "Email", devIds);

            return View(project);
        }

        //[HttpPost]
        //[Authorize(Roles = "Admin, Project Manager")]
        //[ValidateAntiForgeryToken]
        //public ActionResult Archive([Bind(Include = "Id,Name,Description, Archived")] Project project)
        //{

        //    db.Entry(project).Property(x => x.Archived).IsModified = true;
        //    return RedirectToAction("Index", "Home");
        //}

        [HttpPost]
        [Authorize(Roles = "Admin, Project Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description, Archived")] Project project, List<string> Developer, string ProjectManager, string Admin, string Submitter)
        {
            var checktickets = archiveHelper.doesProjectHaveTicketsOpen(project.Id);
            if (checktickets && project.Archived)
            {
                var id = project.Id;
                var users = projectHelper.UsersOnProject(id);
                var pmId = "";
                var adminId = "";
                var sub = "";
                List<string> devIds = new List<string>();

                foreach (var user in users)
                {
                    if (rolerHelpers.IsUserInRole(user.Id, "Project Manager"))
                    {
                        pmId = user.Id;
                    }

                    if (rolerHelpers.IsUserInRole(user.Id, "Developer"))
                    {
                        devIds.Add(user.Id);
                    }
                    if (rolerHelpers.IsUserInRole(user.Id, "Admin"))
                    {
                        adminId = user.Id;
                    }
                    if (rolerHelpers.IsUserInRole(user.Id, "Submitter"))
                    {
                        sub = user.Id;
                    }
                }


                var submitter = rolerHelpers.UsersInRole("Submitter");
                ViewBag.Submitter = new SelectList(submitter, "Id", "Email", sub);

                var admin = rolerHelpers.UsersInRole("Admin");
                ViewBag.Admin = new SelectList(admin, "Id", "Email", adminId);

                var pms = rolerHelpers.UsersInRole("Project Manager");
                ViewBag.ProjectManager = new SelectList(pms, "Id", "Email", pmId);

                var dv = rolerHelpers.UsersInRole("Developer");
                ViewBag.Developer = new MultiSelectList(dv, "Id", "Email", devIds);

                ViewBag.ticketWarning = "The project you are trying to archive has tickets open. Please close all tickets and try again.";
                return View(project);
            }
            if (ModelState.IsValid)
            {
                var users = projectHelper.UsersOnProject(project.Id).ToList();
                var projectmana = "";
                foreach (var use in users)
                {
                    if(rolerHelpers.IsUserInRole(use.Id, "Project Manager"))
                    {
                        projectmana = use.Id;
                    }
                    projectHelper.RemoveUserFromProject(use.Id, project.Id);
                }
                if (Developer != null)
                    foreach (var dev in Developer)
                    {
                        projectHelper.AddUserToProject(dev, project.Id);
                    }
                if (ProjectManager != null)
                    projectHelper.AddUserToProject(ProjectManager, project.Id);
                else
                    projectHelper.AddUserToProject(projectmana, project.Id);

                if (Submitter != null)
                    projectHelper.AddUserToProject(Submitter, project.Id);

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
        public HttpStatusCodeResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
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
