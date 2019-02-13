using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Helpers
{
    public class TicketHistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void AddHistory( int ticketnum,string propertyName, string oldval, string newval)
        {
            var user = HttpContext.Current.User;
            var record = new TicketHistory
            {
                TicketId = ticketnum,
                UserId = user.Identity.GetUserId(),
                PropertyName = propertyName,
                OldValue = oldval,
                NewValue = newval,
                Changed = DateTime.Now
            };
            db.TicketHistories.Add(record);
            db.SaveChanges();
        }

    }
}