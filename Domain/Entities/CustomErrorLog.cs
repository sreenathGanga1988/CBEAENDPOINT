using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class CustomErrorLog : AuditEntity
    {
        public int Id { get; set; }
        public String Detail { get; set; }
        public String log { get; set; }
        public String Reason { get; set; }

        public String Remark { get; set; }

        public String Reference { get; set; }

    }
}
