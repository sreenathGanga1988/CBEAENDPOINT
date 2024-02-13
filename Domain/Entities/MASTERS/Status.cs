using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Status
    {
        public Status()
        {
            Accounts = new HashSet<Accounts>();
            Member = new HashSet<Member>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public int? GroupId { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedByUserId { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual User CreatedByUser { get; set; }
        public virtual User ModifiedByUser { get; set; }
        public virtual ICollection<Accounts> Accounts { get; set; }
        public virtual ICollection<Member> Member { get; set; }
    }
}
