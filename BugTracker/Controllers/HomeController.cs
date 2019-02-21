﻿using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

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
                var tickets = archivedHelper.GetYourActiveTickets(User.Identity.GetUserId()).AsEnumerable<Ticket>(); 

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
            foreach(var project in projects)
            {

                decimal percent = 100 * (project.Tickets.Count() / total);
                output.Add(new projectdata { label = project.Name, value = decimal.ToInt32(decimal.Round(percent)) });
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
                    return (7*i <= daysago && daysago < (i + 2) * 7);

                });
                output.Add(new ticketdata { ticketsubs = ticketcount.Count(), week =  DateTime.Now.AddDays(-7*i).ToString("yyyy-MM-dd") });
                i++;
            } while(i < 12);


            string json = JsonConvert.SerializeObject(output);
            return json;
        }
    }
}