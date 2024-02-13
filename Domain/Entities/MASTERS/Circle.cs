using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Circle
    {
        public Circle()
        {
            Accounts = new HashSet<Accounts>();
            Branch = new HashSet<Branch>();
          
        }

        public int Id { get; set; }
        public int? CircleCode { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public bool IsActive { get; set; }
        public int? StateId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedByUserId { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual User CreatedByUser { get; set; }
        public virtual User ModifiedByUser { get; set; }
        public virtual State State { get; set; }
        public virtual ICollection<Accounts> Accounts { get; set; }
        public virtual ICollection<Branch> Branch { get; set; }
       
    }
}
