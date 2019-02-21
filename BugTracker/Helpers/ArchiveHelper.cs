using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace BugTracker.Helpers
{
    public class ArchiveHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ICollection<Ticket> GetActiveTickets()
        {
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            tickets = tickets.Where(b => b.TicketStatus.Name != "Finished" && b.TicketStatus.Name != "Removed");
            return tickets.ToList();
        }
        public ICollection<Ticket> GetYourActiveTickets(string Id)
        {
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            tickets = tickets.Where(b => b.TicketStatus.Name != "Finished" && b.TicketStatus.Name != "Removed").Where(u => u.AssignedToUserId == Id || u.OwnerUserId == Id);
            return tickets.ToList();
        }
        public ICollection<Ticket> GetArchivedTickets()
        {
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            tickets = tickets.Where(b => b.TicketStatus.Name == "Finished" || b.TicketStatus.Name == "Removed");
            return tickets.ToList();
        }
        public ICollection<Ticket> GetYourArchivedTickets(string Id)
        {
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            tickets = tickets.Where(b => b.TicketStatus.Name == "Finished" || b.TicketStatus.Name == "Removed").Where(u => u.AssignedToUserId == Id || u.OwnerUserId == Id);
            return tickets.ToList();
        }
        public bool doesProjectHaveTicketsOpen(int id)
        {
            var project = db.Projects.Find(id);

            foreach(var ticket in project.Tickets)
            {
                var status = ticket.TicketStatus.ToString();
                if (status == "none" || status == "New" || status == "In Progress" || status == "On Hold")
                {
                    return true;
                }
            }

            return false;
        }
    }
}