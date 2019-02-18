using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper rolesHelper = new UserRoleHelper();
        private ProjectHelper ProjectHelper = new ProjectHelper();
        private TicketNotificationHelper notificationHelper = new TicketNotificationHelper();

        public ActionResult Index()
        {
            if (User.IsInRole("Submitter") || User.IsInRole("Developer"))
            {
                var proj = ProjectHelper.ListUserProjects(User.Identity.GetUserId());
                var userId = User.Identity.GetUserId();
                var tickets = db.Tickets.Where(b => b.AssignedToUserId == userId || b.OwnerUserId == userId).AsEnumerable<Ticket>(); 

                var model = new DashboardMod { Projects = proj, Users = db.Users, Tickets = tickets };
                return View(model);
            }
            else
            {
                var model = new DashboardMod { Projects = db.Projects, Users = db.Users, Tickets = db.Tickets };
                return View(model);
            }
            
        }
        public ActionResult Notifications()
        {   var userId = User.Identity.GetUserId();
            var model = db.TicketNotifications.Where(B => B.RecipientUserId == userId).ToList();
            return View(model);
        }

    }
}