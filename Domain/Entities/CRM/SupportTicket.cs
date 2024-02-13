using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
     public class SupportTicket : AuditEntity
    {
        public int Id { get; set; }

        public String SupportTicketNum { get; set; }

        public String Description { get; set; }

        public String Priority { get; set; }

               public String Duration { get; set; }

        public String DeveloperRemark { get; set; }

        public Boolean isApproved { get; set; }

        public int? ApprovedByUserId { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }
}
