using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ClaimCount : AuditEntity
    {
        public int Id { get; set; }
        public int StateId { get; set; }

        public int YearOF { get; set; }


        public int DesignationId { get; set; }

        public int Count { get; set; }

        
        public string Designation { get; set; }
        public string StateName { get; set; }

    }
}
