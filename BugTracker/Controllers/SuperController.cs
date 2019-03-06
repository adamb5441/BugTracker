using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{

    [Authorize(Roles = "Super Admin")]
    public class SuperController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper rolesHelper = new UserRoleHelper();
        private ProjectHelper ProjectHelper = new ProjectHelper();
        private TicketNotificationHelper notificationHelper = new TicketNotificationHelper();
        private ArchiveHelper archivedHelper = new ArchiveHelper();


        public ActionResult Seed()
        {
            var dev = db.Users.FirstOrDefault(x => x.Email == "dev@mailinator.com");
            var sub = db.Users.FirstOrDefault(x => x.Email == "sub@mailinator.com");
            var manager = db.Users.FirstOrDefault(x => x.Email == "manager@mailinator.com");
           

            db.Projects.Add(new Project { Name = "The keyboard warrior", Description = "React and Node ecommerse site.", Archived = false });
            db.Projects.Add(new Project { Name = "Portfolio", Description = "Jquery and bootstrap site.", Archived = false });
            db.Projects.Add(new Project { Name = "Blog", Description = "MVC5 blog.", Archived = false });
            db.Projects.Add(new Project { Name = "BugTracker", Description = "MVC5 role based functionality app.", Archived = false });
            db.Projects.Add(new Project { Name = "Financial Planner", Description = "MVC5 portion of the fianancial planner.", Archived = false });
            db.SaveChanges();

            var projects = db.Projects;
            foreach( var project in projects)
            {
                ProjectHelper.AddUserToProject(dev.Id, project.Id);
                ProjectHelper.AddUserToProject(sub.Id, project.Id);
                ProjectHelper.AddUserToProject(manager.Id, project.Id);
            }
            var key = db.Projects.FirstOrDefault(m => m.Name == "The keyboard warrior");
            var port = db.Projects.FirstOrDefault(m => m.Name == "Portfolio");
            var blog = db.Projects.FirstOrDefault(m => m.Name == "Blog");
            var bt = db.Projects.FirstOrDefault(m => m.Name == "BugTracker");
            var fp = db.Projects.FirstOrDefault(m => m.Name == "Financial Planner");

            db.Tickets.Add(new Ticket { Title = "Issue hosting project",
                Description = "The store does not render when hosted",
                OwnerUserId = sub.Id,
                AssignedToUserId = dev.Id,
                ProjectId = key.Id,
                Created = DateTime.Now.AddDays(-75),
                TicketPriorityId = 4,
                TicketStatusId = 4,
                TicketTypeId = 5});
            db.Tickets.Add(new Ticket
                {
                    Title = "Local storage cart.",
                    Description = "Cart won't update on local storage.",
                    OwnerUserId = sub.Id,
                    AssignedToUserId = dev.Id,
                    ProjectId = key.Id,
                    Created = DateTime.Now.AddDays(-76),
                    TicketPriorityId = 2,
                    TicketStatusId = 4,
                    TicketTypeId = 5
                });
            db.Tickets.Add(new Ticket
                {
                    Title = "Stripe",
                    Description = "Add stripe payment processor",
                    OwnerUserId = sub.Id,
                    AssignedToUserId = dev.Id,
                    ProjectId = key.Id,
                    Created = DateTime.Now.AddDays(-76),
                    TicketPriorityId = 1,
                    TicketStatusId = 6,
                    TicketTypeId = 4
                });
            db.Tickets.Add(new Ticket
            {
                Title = "Site down",
                Description = "Website showing 404",
                OwnerUserId = sub.Id,
                AssignedToUserId = dev.Id,
                ProjectId = key.Id,
                Created = DateTime.Now.AddDays(-40),
                TicketPriorityId = 4,
                TicketStatusId = 5,
                TicketTypeId = 8
            });
            db.Tickets.Add(new Ticket
            {
                Title = "Update",
                Description = "Add new project",
                OwnerUserId = sub.Id,
                AssignedToUserId = dev.Id,
                ProjectId = port.Id,
                Created = DateTime.Now.AddDays(-1),
                TicketPriorityId = 2,
                TicketStatusId = 2,
                TicketTypeId = 9
            });
            db.Tickets.Add(new Ticket
            {
                Title = "Email",
                Description = "Add email to contact form",
                OwnerUserId = sub.Id,
                AssignedToUserId = dev.Id,
                ProjectId = port.Id,
                Created = DateTime.Now.AddDays(-60),
                TicketPriorityId = 2,
                TicketStatusId = 4,
                TicketTypeId = 2
            });
            db.Tickets.Add(new Ticket
            {
                Title = "Live link drawer",
                Description = "Drawer with live links",
                OwnerUserId = sub.Id,
                AssignedToUserId = dev.Id,
                ProjectId = port.Id,
                Created = DateTime.Now.AddDays(-65),
                TicketPriorityId = 2,
                TicketStatusId = 4,
                TicketTypeId = 2
            });
            db.Tickets.Add(new Ticket
            {
                Title = "Crop images",
                Description = "Crop images to equal size.",
                OwnerUserId = sub.Id,
                AssignedToUserId = dev.Id,
                ProjectId = port.Id,
                Created = DateTime.Now.AddDays(-67),
                TicketPriorityId = 3,
                TicketStatusId = 4,
                TicketTypeId = 2
            });
            db.Tickets.Add(new Ticket
            {
                Title = "Post email animation",
                Description = "Animation confirmation for email",
                OwnerUserId = sub.Id,
                AssignedToUserId = dev.Id,
                ProjectId = port.Id,
                Created = DateTime.Now.AddDays(-55),
                TicketPriorityId = 2,
                TicketStatusId = 6,
                TicketTypeId = 4
            });
            db.Tickets.Add(new Ticket
            {
                Title = "New Blog post",
                Description = "Write a new blog posts",
                OwnerUserId = sub.Id,
                AssignedToUserId = dev.Id,
                ProjectId = blog.Id,
                Created = DateTime.Now.AddDays(0),
                TicketPriorityId = 3,
                TicketStatusId = 2,
                TicketTypeId = 9
            });
            db.Tickets.Add(new Ticket
            {
                Title = "New Blog post",
                Description = "Write a new blog posts",
                OwnerUserId = sub.Id,
                AssignedToUserId = dev.Id,
                ProjectId = blog.Id,
                Created = DateTime.Now.AddDays(-50),
                TicketPriorityId = 3,
                TicketStatusId = 4,
                TicketTypeId = 9
            });
            db.Tickets.Add(new Ticket
            {
                Title = "New Blog post",
                Description = "Write a new blog posts",
                OwnerUserId = sub.Id,
                AssignedToUserId = dev.Id,
                ProjectId = blog.Id,
                Created = DateTime.Now.AddDays(-30),
                TicketPriorityId = 3,
                TicketStatusId = 4,
                TicketTypeId = 9
            });
            db.Tickets.Add(new Ticket
            {
                Title = "Fix Teaxt wrap",
                Description = "Consider if the user enter random charactors",
                OwnerUserId = sub.Id,
                AssignedToUserId = dev.Id,
                ProjectId = blog.Id,
                Created = DateTime.Now.AddDays(-45),
                TicketPriorityId = 2,
                TicketStatusId = 4,
                TicketTypeId = 2
            });
            db.Tickets.Add(new Ticket
            {
                Title = "Comment section",
                Description = "Style comment section.",
                OwnerUserId = sub.Id,
                AssignedToUserId = dev.Id,
                ProjectId = blog.Id,
                Created = DateTime.Now.AddDays(-40),
                TicketPriorityId = 3,
                TicketStatusId = 4,
                TicketTypeId = 2
            });
            db.Tickets.Add(new Ticket
            {
                Title = "Archive",
                Description = "Archive tickets and projects",
                OwnerUserId = sub.Id,
                AssignedToUserId = dev.Id,
                ProjectId = bt.Id,
                Created = DateTime.Now.AddDays(-25),
                TicketPriorityId = 2,
                TicketStatusId = 4,
                TicketTypeId = 4
            });
            db.Tickets.Add(new Ticket
            {
                Title = "Hard delete",
                Description = "Use sweet alert to with hard delete",
                OwnerUserId = sub.Id,
                AssignedToUserId = dev.Id,
                ProjectId = bt.Id,
                Created = DateTime.Now.AddDays(-20),
                TicketPriorityId = 3,
                TicketStatusId = 4,
                TicketTypeId = 4
            });
            db.Tickets.Add(new Ticket
            {
                Title = "Final Check",
                Description = "Walk threw check everything",
                OwnerUserId = sub.Id,
                AssignedToUserId = dev.Id,
                ProjectId = bt.Id,
                Created = DateTime.Now.AddDays(-15),
                TicketPriorityId = 4,
                TicketStatusId = 4,
                TicketTypeId = 1
            });
            db.Tickets.Add(new Ticket
            {
                Title = "Scaling Issues",
                Description = "Nav doesn't change and tables are off the screen",
                OwnerUserId = sub.Id,
                AssignedToUserId = dev.Id,
                ProjectId = bt.Id,
                Created = DateTime.Now.AddDays(-9),
                TicketPriorityId = 3,
                TicketStatusId = 4,
                TicketTypeId = 4
            });
            db.Tickets.Add(new Ticket
            {
                Title = "Seed methods.",
                Description = "Need to write better seed methods.",
                OwnerUserId = sub.Id,
                AssignedToUserId = dev.Id,
                ProjectId = bt.Id,
                Created = DateTime.Now.AddDays(0),
                TicketPriorityId = 3,
                TicketStatusId = 3,
                TicketTypeId = 5
            });
            db.Tickets.Add(new Ticket
            {
                Title = "Scafold and set up invite",
                Description = "Week 9 deliverables",
                OwnerUserId = sub.Id,
                AssignedToUserId = dev.Id,
                ProjectId = fp.Id,
                Created = DateTime.Now.AddDays(-5),
                TicketPriorityId = 4,
                TicketStatusId = 4,
                TicketTypeId = 2
            });
            db.Tickets.Add(new Ticket
            {
                Title = "Budget and transaction",
                Description = "Week 10 deliverables",
                OwnerUserId = sub.Id,
                AssignedToUserId = dev.Id,
                ProjectId = fp.Id,
                Created = DateTime.Now.AddDays(-1),
                TicketPriorityId = 4,
                TicketStatusId = 2,
                TicketTypeId = 2
            });
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}