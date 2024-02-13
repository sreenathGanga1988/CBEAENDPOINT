using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Category:AuditEntity
    {
        public Category()
        {
            Member = new HashSet<Member>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
      
        public virtual ICollection<Member> Member { get; set; }
    }
}
