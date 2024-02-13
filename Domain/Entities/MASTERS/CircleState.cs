using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CircleState
    {
        public int CircleId { get; set; }
        public int StateId { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedByUserId { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Circle Circle { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual User ModifiedByUser { get; set; }
        public virtual State State { get; set; }
    }
}
