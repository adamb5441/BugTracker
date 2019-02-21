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
        private ProjectHelper projectHelper = new ProjectHelper();
        public ICollection<Ticket> GetActiveTickets()
        {
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            tickets = tickets.Where(b => b.TicketStatus.Name != "Finished" && b.TicketStatus.Name != "Removed");
            return tickets.ToList();
        }
        public ICollection<Ticket> GetYourActiveTickets(string Id)
        {
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            tickets = tickets.Where(b => b.TicketStatus.Name != "Finished" && b.TicketStatus.Name != "Removed" && (b.OwnerUserId == Id || b.AssignedToUserId == Id) );
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
        public ICollection<Ticket> ProjectsActiveTickets(int Id)
        {
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            tickets = tickets.Where(b => b.TicketStatus.Name != "Finished" && b.TicketStatus.Name != "Removed" && b.ProjectId == Id);
            return tickets.ToList();
        }
        public bool doesProjectHaveTicketsOpen(int id)
        {
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            tickets = tickets.Where(t => t.ProjectId == id);

            foreach (var ticket in tickets.ToList())
            {
                var status = ticket.TicketStatus.Name.ToString();
                if (status == "none" || status == "New" || status == "In Progress" || status == "On Hold")
                {
                    return true;
                }
            }

            return false;
        }
        public ICollection<Project> GetActiveProjects()
        {
            var projects = db.Projects.Where(b => !b.Archived);
            return projects.ToList();
        }
        public ICollection<Project> GetArchivedProjects()
        {
            var projects = db.Projects.Where(b => b.Archived);
            return projects.ToList();
        }
        public ICollection<Project> GetMyActiveProjects(string id)
        {
            var projects = projectHelper.ListUserProjects(id).Where(b => !b.Archived);
            return projects.ToList();
        }
        public ICollection<Project> GetMyArchivedProjects(string id)
        {
            var projects = projectHelper.ListUserProjects(id).Where(b => b.Archived);
            return projects.ToList();
        }
    }
}