using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class State
    {
        public State()
        {
            Branch = new HashSet<Branch>();
            Circle = new HashSet<Circle>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedByUserId { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Branch> Branch { get; set; }
        public virtual ICollection<Circle> Circle { get; set; }
    }
}
