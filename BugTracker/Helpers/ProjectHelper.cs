using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class ProjectHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            bool flag = project.Users.Any(u=>u.Id==userId);
            return (flag);
        }

        public ICollection<Project> ListUserProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var projects = user.Projects.ToList();
            return (projects);
        }

        public void AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var newUser = db.Users.Find(userId);

                proj.Users.Add(newUser);
                db.SaveChanges();
            }
        }

        public void RemoveUserFromProject(string userId, int projectId)
        {
            if (IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var oldUser = db.Users.Find(userId);

                proj.Users.Remove(oldUser);
                db.Entry(proj).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public ICollection<ApplicationUser> UsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).Users;
        }
        public ICollection<ApplicationUser> UsersNotOnProject(int projectId)
        {
            return
                db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        }
        public ICollection<ApplicationUser> UsersOnProjectWithRole(string role, int projectId)
        {
            var users = new List<ApplicationUser>();
            var projUsers = UsersOnProject(projectId);
            var roleHelper = new UserRoleHelper();
            foreach (var user in projUsers)
            {
                if (roleHelper.IsUserInRole(user.Id, role))
                {
                    users.Add(user);
                }
            }
            return users;
        }
    }
}