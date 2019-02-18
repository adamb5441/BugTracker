using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Helpers;
using System.Threading.Tasks;

namespace BugTracker.Helpers
{
    public class TicketChangeHelper
    {
        private static TicketHistoryHelper historyHelper = new TicketHistoryHelper();
        private static TicketNotificationHelper NotificationHelper = new TicketNotificationHelper();

        public async Task TicketChangeAsync(Ticket OldTicket, Ticket NewTicket)
        {
            var currentUser = HttpContext.Current.User;
            var devid = NewTicket.AssignedToUserId;
            if (!currentUser.IsInRole("Developer") && !currentUser.IsInRole("Submitter"))
            {
                if (OldTicket.AssignedToUserId != NewTicket.AssignedToUserId)
                {
                    if (OldTicket.AssignedToUserId == null)
                    {
                        devid = NewTicket.AssignedToUserId;
                        if(devid != null)
                        await NotificationHelper.SendNotificationAsync("You hav been assigned to ticket","Assignment", NewTicket.Id, devid);
                    }
                    else
                    {
                        devid = OldTicket.AssignedToUserId;
                        var devid2 = NewTicket.AssignedToUserId;
                        if (devid != null)
                            await NotificationHelper.SendNotificationAsync("You hav been unassigned from ticket", "Assignment", NewTicket.Id, devid);
                        if (devid2 != null)
                            await NotificationHelper.SendNotificationAsync("You hav been assigned to ticket", "Assignment", NewTicket.Id, devid2);

                    }
                    historyHelper.AddHistory(NewTicket.Id, "Assignment", OldTicket.AssignedToUserId, NewTicket.AssignedToUserId);
                }
            }
            if (OldTicket.Title != NewTicket.Title)
            {
                historyHelper.AddHistory(NewTicket.Id, "Title", OldTicket.Title, NewTicket.Title);
            }
            if (OldTicket.Description != NewTicket.Description)
            {
                historyHelper.AddHistory(NewTicket.Id, "Description", OldTicket.Description, NewTicket.Description);
            }
            if (OldTicket.TicketStatusId != NewTicket.TicketStatusId && currentUser.IsInRole("Admin") || currentUser.IsInRole("Project Manager"))
            {
                if (devid != null)
                    await NotificationHelper.SendNotificationAsync("The status has changed for ticket", "Status", NewTicket.Id, devid);
                historyHelper.AddHistory(NewTicket.Id, "TicketStatus", OldTicket.TicketStatusId.ToString(), NewTicket.TicketStatusId.ToString());

            }
            if (OldTicket.TicketPriorityId != NewTicket.TicketPriorityId)
            {
                if (devid != null)
                    await NotificationHelper.SendNotificationAsync("The Priority has changed for ticket", "Priority", NewTicket.Id, devid);
                historyHelper.AddHistory(NewTicket.Id, "TicketPriority", OldTicket.TicketPriorityId.ToString(), NewTicket.TicketPriorityId.ToString());
            }
            if (OldTicket.TicketTypeId != NewTicket.TicketTypeId)
            {
                historyHelper.AddHistory(NewTicket.Id, "TicketType", OldTicket.TicketTypeId.ToString(), NewTicket.TicketTypeId.ToString());
            }

        }
    }
}