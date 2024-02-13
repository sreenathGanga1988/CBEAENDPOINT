using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities

{
    public partial class UserLog
    {
        public int Id { get; set; }
        public int userid { get; set; }

        public DateTime ActionTimeUTC { get; set; }

        public String ActionType { get; set; }

    }
}
