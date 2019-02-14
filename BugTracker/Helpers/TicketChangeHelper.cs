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
            var devid = NewTicket.AssignedToUserId;
            if (OldTicket.AssignedToUserId != NewTicket.AssignedToUserId)
            {
                if (OldTicket.AssignedToUserId == null)
                {
                    devid = NewTicket.AssignedToUserId;
                    await NotificationHelper.SendNotificationAsync("Assignment", NewTicket.Id, devid);
                }
                else
                {
                    devid = OldTicket.AssignedToUserId;
                    var devid2 = NewTicket.AssignedToUserId;
                    await NotificationHelper.SendNotificationAsync("Assignment", NewTicket.Id, devid);
                    await NotificationHelper.SendNotificationAsync("Assignment", NewTicket.Id, devid2);

                }
                historyHelper.AddHistory(NewTicket.Id, "Assignment", OldTicket.AssignedToUserId, NewTicket.AssignedToUserId);
            }

            if (OldTicket.Title != NewTicket.Title)
            {
                historyHelper.AddHistory(NewTicket.Id, "Title", OldTicket.Title, NewTicket.Title);
            }
            if (OldTicket.Description != NewTicket.Description)
            {
                historyHelper.AddHistory(NewTicket.Id, "Description", OldTicket.Description, NewTicket.Description);
            }
            if (OldTicket.TicketStatusId != NewTicket.TicketStatusId)
            {
                await NotificationHelper.SendNotificationAsync("Status", NewTicket.Id, devid);
                historyHelper.AddHistory(NewTicket.Id, "TicketStatus", OldTicket.TicketStatusId.ToString(), NewTicket.TicketStatusId.ToString());

            }
            if (OldTicket.TicketPriorityId != NewTicket.TicketPriorityId)
            {
                await NotificationHelper.SendNotificationAsync("Priority", NewTicket.Id, devid);
                historyHelper.AddHistory(NewTicket.Id, "TicketPriority", OldTicket.TicketPriorityId.ToString(), NewTicket.TicketPriorityId.ToString());
            }
            if (OldTicket.TicketTypeId != NewTicket.TicketTypeId)
            {
                await NotificationHelper.SendNotificationAsync("Type", NewTicket.Id, devid);
                historyHelper.AddHistory(NewTicket.Id, "TicketType", OldTicket.TicketTypeId.ToString(), NewTicket.TicketTypeId.ToString());
            }

        }
    }
}