using Domain.Entities;
using System;

public  class RefundContribution : AuditEntity
    {

       
        public int Id { get; set; }
        public int StaffNo { get; set; }

        public int? StateId { get; set; }

        public int? DesignationId { get; set; }
        public DateTime? DeathDate { get; set; }

        public String RefundNO { get; set; }
        public String BranchNameOFTime { get; set; }
        public String DPCODEOfTime { get; set; }

        public String Type { get; set; }

        public String Remark { get; set; }
        public string DDNO { get; set; }

        public DateTime? DDDATE { get; set; }


        public decimal Amount { get; set; }

        public float LastContribution { get; set; }


        public int YearOF { get; set; }

        public virtual State State { get; set; }
          public virtual Member StaffNoNavigation { get; set; }

        public virtual Designation Designation { get; set; }
    }