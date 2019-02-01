using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketNotification
    {
        public int Id { get; set; }

        public int TicketId { get; set; }
        public string SenderUserId { get; set; }
        public string RecipientUserId { get; set; }

        public string NotifocationBody { get; set; }
        public DateTime Created { get; set; }
        
        public bool Comfirmed { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser SenderUser { get; set; }
        public virtual ApplicationUser RecipientUser { get; set; }
    }
}