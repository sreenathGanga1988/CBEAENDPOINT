using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class Branch :AuditEntity
    {
        public Branch()
        {
            Accounts = new HashSet<Accounts>();
            Member = new HashSet<Member>();
            Transfer = new HashSet<Transfer>();
        }

        public int Id { get; set; }
        public int DpCode { get; set; }
        public int CircleId { get; set; }
        public int? StateId { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string District { get; set; }
       
        public bool IsRegCompleted { get; set; }
    
        public string Status { get; set; }
        public virtual Circle Circle { get; set; }
 
        public virtual State State { get; set; }
        public virtual ICollection<Accounts> Accounts { get; set; }
        public virtual ICollection<Member> Member { get; set; }
        public virtual ICollection<Transfer> Transfer { get; set; }

        [NotMapped]
        public string Circle_text { get; set; }
        [NotMapped]
        public string State_text { get; set; }

      
    }
}
