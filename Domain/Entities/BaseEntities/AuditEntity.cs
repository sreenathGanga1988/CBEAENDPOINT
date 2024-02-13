using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class AuditEntity
    {



        public bool IsActive { get; set; } = true;
        public int? CreatedByUserId { get; set; }
        public string AddedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedByUserId { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int? DeletedByByUserId { get; set; }
        public string DeletedUser { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual User CreatedByUser { get; set; }
        public virtual User ModifiedByUser { get; set; }
     



    }
}
