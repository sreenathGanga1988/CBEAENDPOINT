using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
   public  class DeathClaim : AuditEntity
    {

       
        public int Id { get; set; }
        public int StaffNo { get; set; }

        public int? StateId { get; set; }

        public int? DesignationId { get; set; }
        public DateTime? DeathDate { get; set; }

        public string Nominee { get; set; }

        public string NomineeRelation { get; set; }

        public string NomineeIDentity { get; set; }


        public string DDNO { get; set; }

        public DateTime? DDDATE { get; set; }


        public decimal Amount { get; set; }

        public float LastContribution { get; set; }


        public int YearOF { get; set; }

        public virtual State State { get; set; }
          public virtual Member StaffNoNavigation { get; set; }

        public virtual Designation Designation { get; set; }
    }
}
