namespace BugTracker.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

   
        public class Ticket
        {
            public int Id { get; set; }
            public int ProjectId { get; set; }
            
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Updated { get; set; }

            public string OwnerUserId { get; set; }
            public string AssignedToUserId { get; set; }


            public int TicketStatusId { get; set; }
            public int TicketPriorityId { get; set; }
            public int TicketTypeId { get; set; }


            public virtual Project Project { get; set; }
            public virtual ApplicationUser OwnerUser { get; set; }
            public virtual ApplicationUser AssignedToUser { get; set; }

            public virtual TicketStatus TicketStatus { get; set; }
            public virtual TicketPriority TicketPriority { get; set; }
            public virtual TicketType TicketType { get; set; }

            
            public ICollection<TicketAttachment> TicketAttachments { get; set; }
            public ICollection<TicketComment> TicketComments { get; set; }
            public ICollection<TicketHistory> TicketHistories { get; set; }
            public ICollection<TicketNotification> TicketNotifications { get; set; }

        public Ticket()
        {
            this.TicketAttachments = new HashSet<TicketAttachment>();
            this.TicketComments = new HashSet<TicketComment>();
            this.TicketHistories = new HashSet<TicketHistory>();
            this.TicketNotifications = new HashSet<TicketNotification>();
        }
    }

}