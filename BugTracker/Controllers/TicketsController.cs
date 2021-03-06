﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using BugTracker.Helpers;
using System.Threading.Tasks;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper projectHelper = new ProjectHelper();
        private UserRoleHelper userRoleHelper = new UserRoleHelper();
        private TicketChangeHelper TicketWasChanged = new TicketChangeHelper();
        private ArchiveHelper archivedHelper = new ArchiveHelper();


        public ActionResult Index()
        { 
            return View(archivedHelper.GetActiveTickets());
        }
        public ActionResult Archive()
        {
            if (User.IsInRole("Developer") || User.IsInRole("Submitter"))
            {
                return View(archivedHelper.GetYourArchivedTickets(User.Identity.GetUserId()));
            }
            return View(archivedHelper.GetArchivedTickets());
        }
        public ActionResult Details(int id)
        {
            Ticket ticketref = db.Tickets.Find(id);
            var userId = User.Identity.GetUserId();
            var proj = ticketref.ProjectId;
            if (User.IsInRole("Submitter") && userId != ticketref.OwnerUserId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (User.IsInRole("Developer") && userId != ticketref.AssignedToUserId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            var isuseronproject = projectHelper.IsUserOnProject(userId, proj);
            if (User.IsInRole("Project Manager") && !isuseronproject)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            Ticket ticket = db.Tickets.Include(t => t.TicketHistories).Include(t => t.TicketAttachments).Include(t => t.TicketComments).FirstOrDefault(t => t.Id == id);

            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create()
        {



            //ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName");
            //ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName");
            var userId = User.Identity.GetUserId();
            var projId = archivedHelper.GetMyActiveProjects(userId);
            ViewBag.ProjectId = new SelectList(projId, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Submitter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectId,Title,Description,OwnerUserId,AssignedToUserId,TicketPriorityId,TicketTypeId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.Created = DateTime.Now;
                ticket.TicketStatusId = 2;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            var userId = User.Identity.GetUserId();
            var projId = archivedHelper.GetMyActiveProjects(userId);
            ViewBag.ProjectId = new SelectList(projId, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }
        [Authorize]
        // GET: Tickets/Edit/5
        public ActionResult Edit(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            var userId = User.Identity.GetUserId();
            var proj = ticket.ProjectId;

            if (User.IsInRole("Submitter") && userId != ticket.OwnerUserId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (User.IsInRole("Developer") && userId != ticket.AssignedToUserId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var isuseronproject = projectHelper.IsUserOnProject(userId, proj);
            if (User.IsInRole("Project Manager") && !isuseronproject)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            if (ticket == null)
            {
                return HttpNotFound();
            }

            var projUsers = projectHelper.UsersOnProject(ticket.ProjectId);
            //var devs = projUsers.Where(B => UserRoleHelper.IsUserInRole(B.Id, "Developer"));

            var devs = projectHelper.UsersOnProjectWithRole("Developer", ticket.ProjectId);

            ViewBag.AssignedToUserId = new SelectList(devs, "Id", "Email", ticket.AssignedToUserId);
            if (User.IsInRole("Submitter"))
            {
                var projId = archivedHelper.GetMyActiveProjects(userId);
                ViewBag.ProjectId = new SelectList(projId, "Id", "Name", ticket.ProjectId);
            }
            else
            {
                var projId = archivedHelper.GetActiveProjects();
                ViewBag.ProjectId = new SelectList(projId, "Id", "Name", ticket.ProjectId);
            }
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProjectId,Title,Description,Created,Updated,OwnerUserId,AssignedToUserId,TicketStatusId,TicketPriorityId,TicketTypeId")] Ticket ticket)
        {
            var oldticket = db.Tickets.AsNoTracking().FirstOrDefault(B => B.Id == ticket.Id);
            var userId = User.Identity.GetUserId();
            var proj = oldticket.ProjectId;
            var owner = oldticket.OwnerUserId;
            if (User.IsInRole("Submitter") && userId != owner)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var assigned = oldticket.AssignedToUserId;
            if (User.IsInRole("Developer") && userId != assigned)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var isOnProject = projectHelper.IsUserOnProject(userId, proj);
            if (User.IsInRole("Project Manager") && !isOnProject)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                ticket.Updated = DateTime.Now;
                
                ticket.OwnerUserId = oldticket.OwnerUserId;
                await TicketWasChanged.TicketChangeAsync(oldticket, ticket);

                db.Tickets.Attach(ticket);
                if (User.IsInRole("Admin") || User.IsInRole("Project Manager"))
                {
                    db.Entry(ticket).Property(x => x.AssignedToUserId).IsModified = true;
                    db.Entry(ticket).Property(x => x.TicketStatusId).IsModified = true;
                }
                    db.Entry(ticket).Property(x => x.Title).IsModified = true;
                    db.Entry(ticket).Property(x => x.Description).IsModified = true;
                    db.Entry(ticket).Property(x => x.TicketPriorityId).IsModified = true;
                    db.Entry(ticket).Property(x => x.TicketTypeId).IsModified = true;
                
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            var projUsers = projectHelper.UsersOnProject(ticket.Id);
            var devs = projUsers.Where(B => userRoleHelper.IsUserInRole(B.Id, "Developer"));

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "Email", devs);
            if (User.IsInRole("Admin"))
            {
                var projId = archivedHelper.GetActiveProjects();
                ViewBag.ProjectId = new SelectList(projId, "Id", "Name", ticket.ProjectId);
            }
            else
            {
                var projId = archivedHelper.GetMyActiveProjects(userId);
                ViewBag.ProjectId = new SelectList(projId, "Id", "Name", ticket.ProjectId);
            }
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public HttpStatusCodeResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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
