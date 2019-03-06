namespace BugTracker.Migrations
{
    using BugTracker.Helpers;
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
        private ProjectHelper projectHelper = new ProjectHelper();

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Super Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Super Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "adamb5441@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "adamb5441@gmail.com",
                    Email = "adamb5441@gmail.com",
                    FirstName = "Adam",
                    LastName = "Brown",
                    DisplayName = "ABrown"
                }, "Abc!123");
            }

            var AdminuserId = userManager.FindByEmail("adamb5441@gmail.com").Id;
            userManager.AddToRole(AdminuserId, "Admin");

            if (!context.Users.Any(u => u.Email == "adam@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "adam@mailinator.com",
                    Email = "adam@mailinator.com",
                    FirstName = "Adam",
                    LastName = "Brown",
                    DisplayName = "Super Admin"
                }, "Abc!1234");
            }

            var SuperuserId = userManager.FindByEmail("adam@mailinator.com").Id;
            userManager.AddToRole(SuperuserId, "Super Admin");

            if (!context.Users.Any(u => u.Email == "twichell@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "twichell@mailinator.com",
                    Email = "twichell@mailinator.com",
                    FirstName = "Jason",
                    LastName = "Twichell",
                    DisplayName = " Twich"
                }, "Abc&123!");
            }

            var demodevId = userManager.FindByEmail("twichell@mailinator.com").Id;
            userManager.AddToRole(demodevId, "Developer");


            if (!context.Users.Any(u => u.Email == "dev@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "dev@mailinator.com",
                    Email = "dev@mailinator.com",
                    FirstName = "Andrew",
                    LastName = "Zimmerman",
                    DisplayName = " AZimmerman"
                }, "Abc!123");
            }

            var devId = userManager.FindByEmail("dev@mailinator.com").Id;
            userManager.AddToRole(devId, "Developer");

            if (!context.Users.Any(u => u.Email == "dev2@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "dev2@mailinator.com",
                    Email = "dev2@mailinator.com",
                    FirstName = "Nancy ",
                    LastName = "Baumgartner",
                    DisplayName = " NancyM"
                }, "Abc!123");
            }

            devId = userManager.FindByEmail("dev2@mailinator.com").Id;
            userManager.AddToRole(devId, "Developer");

            if (!context.Users.Any(u => u.Email == "dev3@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "dev3@mailinator.com",
                    Email = "dev3@mailinator.com",
                    FirstName = "Kate",
                    LastName = "Tweedie",
                    DisplayName = " Katy"
                }, "Abc!123");
            }

            devId = userManager.FindByEmail("dev3@mailinator.com").Id;
            userManager.AddToRole(devId, "Developer");

            if (!context.Users.Any(u => u.Email == "sub@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "sub@mailinator.com",
                    Email = "sub@mailinator.com",
                    FirstName = "Jordon",
                    LastName = "Marron",
                    DisplayName = "JordonM"
                }, "Abc!123");
            }

            var demosubId = userManager.FindByEmail("sub@mailinator.com").Id;
            userManager.AddToRole(demosubId, "Submitter");

            if (!context.Users.Any(u => u.Email == "sub2@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "sub2@mailinator.com",
                    Email = "sub2@mailinator.com",
                    FirstName = "Shawn",
                    LastName = "Singer",
                    DisplayName = "Shawn"
                }, "Abc!123");
            }

            var subId = userManager.FindByEmail("sub2@mailinator.com").Id;
            userManager.AddToRole(subId, "Submitter");

            if (!context.Users.Any(u => u.Email == "sub3@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "sub3@mailinator.com",
                    Email = "sub3@mailinator.com",
                    FirstName = "Alfredo",
                    LastName = "Acosta",
                    DisplayName = "Alfredo"
                }, "Abc!123");
            }

            subId = userManager.FindByEmail("sub3@mailinator.com").Id;
            userManager.AddToRole(subId, "Submitter");

            if (!context.Users.Any(u => u.Email == "manager@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "manager@mailinator.com",
                    Email = "manager@mailinator.com",
                    FirstName = "Reed",
                    LastName = "Laverty",
                    DisplayName = "Reed"
                }, "Abc!123");
            }

            var demomanaId = userManager.FindByEmail("manager@mailinator.com").Id;
            userManager.AddToRole(demomanaId, "Project Manager");

            if (!context.Users.Any(u => u.Email == "manager2@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "manager2@mailinator.com",
                    Email = "manager2@mailinator.com",
                    FirstName = "Barbara",
                    LastName = "Marie",
                    DisplayName = "Barbara"
                }, "Abc!123");
            }

            var manaId = userManager.FindByEmail("manager2@mailinator.com").Id;
            userManager.AddToRole(manaId, "Project Manager");

            if (!context.Users.Any(u => u.Email == "manager3@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "manager3@mailinator.com",
                    Email = "manager3@mailinator.com",
                    FirstName = "Kevin",
                    LastName = "Torres",
                    DisplayName = "Kevin"
                }, "Abc!123");
            }

            manaId = userManager.FindByEmail("manager3@mailinator.com").Id;
            userManager.AddToRole(manaId, "Project Manager");

            context.TicketPriorities.AddOrUpdate(
             B => B.Name,
                    new TicketPriority { Name = "none"},
                    new TicketPriority { Name = "Low"},
                    new TicketPriority { Name = "Medium"},
                    new TicketPriority { Name = "High" },
                    new TicketPriority { Name = "Emergency" }
            );
            context.TicketStatuses.AddOrUpdate(
            B => B.Name,
                new TicketStatus {  Name = "none" },
                new TicketStatus { Name = "New" },
                new TicketStatus { Name = "In Progress" },
                new TicketStatus { Name = "Finished" },
                new TicketStatus { Name = "On Hold" },
                new TicketStatus { Name = "Removed" }
            );
            context.TicketTypes.AddOrUpdate(
                B => B.Name,
                new TicketType { Name = "none" },
                new TicketType { Name = "MVP" },
                new TicketType { Name = "Redesign" },
                new TicketType { Name = "Extra Feature" },
                new TicketType { Name = "Bug" },
                new TicketType { Name = "Documentation Requestion" },
                new TicketType { Name = "Training Request" },
                new TicketType { Name = "System Down" },
                new TicketType { Name = "Update" }

            );

        }

    }
}

