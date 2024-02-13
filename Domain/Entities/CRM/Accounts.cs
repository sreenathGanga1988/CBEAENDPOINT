using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Accounts
    {
        public long Id { get; set; }
        public int CircleId { get; set; }
        public int? DpCode { get; set; }
        public int StaffNo { get; set; }
        public int MonthCode { get; set; }
        public int YearOf { get; set; }
        public decimal Amount { get; set; }
        public int TransMode { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedByUserId { get; set; }
        public DateTime ModifiedDate { get; set; }


       

        public string Reference { get; set; }
        public string Remark { get; set; }
        public virtual Circle Circle { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual Branch DpCodeNavigation { get; set; }
        public virtual User ModifiedByUser { get; set; }
        public virtual Member StaffNoNavigation { get; set; }
        public virtual Status TransModeNavigation { get; set; }


    }
}
