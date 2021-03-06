﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helpers;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: TicketAttachments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Create
        public ActionResult Create()
        {
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int TicketId, string AttachDescription, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                var ticketAttachment = new TicketAttachment
                {
                    Created = DateTime.Now,
                    TicketId = TicketId,
                    UserId = User.Identity.GetUserId(),
                    Description = AttachDescription


                };
                var ticket = db.Tickets.Find(TicketId);
                if (AssetHelper.IsWebFriendlyImage(image))
                {
                    var filename = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), filename));
                    ticketAttachment.FilePath = "/Uploads/" + filename;
                }
                if (ticket.AssignedToUserId != null)
                {
                    TicketNotificationHelper NotificationHelper = new TicketNotificationHelper();
                    await NotificationHelper.SendNotificationAsync("File added ticket","A attachment has been added to ticket", "Ticket Attachment", ticket.Id, ticket.AssignedToUserId);
                }

                db.TicketAttachments.Add(ticketAttachment);
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { Id = TicketId });
            }

            return RedirectToAction("Details", "Tickets", new { Id = TicketId });
        }



        // GET: TicketAttachments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int attachmentId, int origin)
        {
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(attachmentId);
            db.TicketAttachments.Remove(ticketAttachment);
            db.SaveChanges();
            return RedirectToAction("Details", "Tickets", new { id = origin });
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
