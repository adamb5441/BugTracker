using BugTracker.Models;
using System.Collections.Generic;

namespace BugTracker.Models
{
    public class ProjectandUsers
    {
        public IEnumerable<Project> Projects {get; set;}
        public IEnumerable<ApplicationUser> Users {get; set;}
    }
}