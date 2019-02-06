using BugTracker.Models;
using System.Collections.Generic;

namespace BugTracker.Models
{
    public class DashboardMod
    {
        public IEnumerable<Project> Projects {get; set;}
        public IEnumerable<ApplicationUser> Users {get; set;}
        public IEnumerable<Ticket> Tickets {get; set; }
    }
}