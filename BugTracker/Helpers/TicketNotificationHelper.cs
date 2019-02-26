using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using BugTracker.Models;
using Microsoft.AspNet.Identity;


namespace BugTracker.Helpers
{
    public class TicketNotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public async System.Threading.Tasks.Task SendNotificationAsync( string shortbody,string notification,string update, int ticketnum, string devid)
        {
            
            var from = "BugTracker<Bug@Track.com>";
            var userId = HttpContext.Current.User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(userId);
            var emailfrom = user.Email;
            ApplicationUser userto = db.Users.Find(devid);
            var emailto = userto.Email;
            var ticketname = db.Tickets.Find(ticketnum).Title;
            var body = $"{notification} {ticketname}. Please contact a supervisor for further questions.";

            var email = new MailMessage(from, emailto)
            {
                Subject = "Ticket changes",
                Body = body,
                IsBodyHtml = true
            };

            var svc = new PersonalEmail();
            await svc.SendAsync(email);
            var dropbody = notification + " " + ticketname;
            var record = new TicketNotification
            {
                ShortBody = shortbody,
                TicketId = ticketnum,
                SenderUserId = userId,
                RecipientUserId = devid,
                Created = DateTime.Now,
                Comfirmed = false,
                NotifocationBody = dropbody,
            };
            db.TicketNotifications.Add(record);
            db.SaveChanges();
        }
        public ICollection<TicketNotification> GetNotification()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            return db.TicketNotifications.Where(B => B.RecipientUserId == userId).ToList();
        }
    }
}