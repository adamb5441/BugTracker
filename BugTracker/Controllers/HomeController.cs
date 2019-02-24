using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System.Net;

namespace BugTracker.Controllers
{
    
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper rolesHelper = new UserRoleHelper();
        private ProjectHelper ProjectHelper = new ProjectHelper();
        private TicketNotificationHelper notificationHelper = new TicketNotificationHelper();
        private ArchiveHelper archivedHelper = new ArchiveHelper();

        public ActionResult Index()
        {
            if (User.IsInRole("Submitter") || User.IsInRole("Developer"))
            {
                var proj = ProjectHelper.ListUserProjects(User.Identity.GetUserId());
                var userId = User.Identity.GetUserId();
                var tickets = archivedHelper.GetYourActiveTickets(User.Identity.GetUserId()); 

                var model = new DashboardMod { Projects = archivedHelper.GetMyActiveProjects(userId), Users = db.Users, Tickets = tickets };
                return View(model);
            }
            else
            {
                var model = new DashboardMod { Projects = archivedHelper.GetActiveProjects(), Users = db.Users, Tickets = archivedHelper.GetActiveTickets()};
                return View(model);
            }
            
        }
        public ActionResult Notifications()
        {   var userId = User.Identity.GetUserId();
            var model = db.TicketNotifications.Where(B => B.RecipientUserId == userId).ToList();
            return View(model);
        }
        public class ticketdata
        {
            
            public string week { get; set; }
            public int ticketsubs { get; set; }
        }
        public class projectdata
        {

            public string label { get; set; }
            public int value { get; set; }
        }
        [Authorize]
        public string ProjectData()
        {
            var output = new List<projectdata>();
            var projects = db.Projects.Include("Tickets").ToList();
            var tickets = db.Tickets.ToList();
            decimal total = tickets.Count();
            if (total == 0)
            {
                output.Add(new projectdata { label = "Project data will display here.", value = 100 });
            }
            else
            {
                foreach (var project in projects)
                {

                    decimal percent = 100 * (project.Tickets.Count() / total);
                    output.Add(new projectdata { label = project.Name, value = decimal.ToInt32(decimal.Round(percent)) });
                }
            }
            string json = JsonConvert.SerializeObject(output);
            return json;
        }
        [Authorize]
        public string YourProjectData()
        {
            var userId = User.Identity.GetUserId(); 
            var output = new List<projectdata>();
            var projects = archivedHelper.GetMyActiveProjects(userId);
            var tickets = archivedHelper.GetYourActiveTickets(userId);
            decimal total = tickets.Count();
            if (total == 0)
            {
                output.Add(new projectdata { label = "Project data will display here.", value = 100 });
            }
            else
            {
                foreach (var project in projects)
                {
                        decimal percent = 100 * (project.Tickets.Count() / total);
                        output.Add(new projectdata { label = project.Name, value = decimal.ToInt32(decimal.Round(percent)) });
                    
                }
            }
            string json = JsonConvert.SerializeObject(output);

            return json;
        }
        [Authorize]
        public string TicketData()
        {
            
            var tickets = db.Tickets.ToList();
            var output = new List<ticketdata>();
            int i= 0;

            do
            {
                var ticketcount = tickets.Where(B => {
                    var daysago = Int32.Parse(DateTimeOffset.Now.Subtract(B.Created).ToString("%d"));
                    return (7*i <= daysago && daysago < (i + 1) * 7);

                });
                output.Add(new ticketdata { ticketsubs = ticketcount.Count(), week =  DateTime.Now.AddDays(-7*i).ToString("yyyy-MM-dd") });
                i++;
            } while(i < 12);


            string json = JsonConvert.SerializeObject(output);
            return json;
        }
        [HttpPost]
        [Authorize]
        public HttpStatusCodeResult ConfirmViewed()
        {
            var userId = User.Identity.GetUserId();
            var notifications = db.TicketNotifications.Where(n => n.RecipientUserId == userId);
            foreach(TicketNotification notification in notifications)
            {
                notification.Comfirmed = true;
                db.Entry(notification).Property(x => x.Comfirmed).IsModified = true;
            }

            db.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}