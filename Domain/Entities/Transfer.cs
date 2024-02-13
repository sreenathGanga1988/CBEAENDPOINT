using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Transfer
    {
        public int Id { get; set; }
        public int StaffNo { get; set; }
        public int DpCode { get; set; }
        public DateTime? TransDate { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedByUserId { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual User CreatedByUser { get; set; }
        public virtual Branch DpCodeNavigation { get; set; }
        public virtual User ModifiedByUser { get; set; }
        public virtual Member StaffNoNavigation { get; set; }
    }
}
