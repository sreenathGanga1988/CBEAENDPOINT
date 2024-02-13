using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Month
    {
        public Month()
        {
          
        }
        public int MonthCode { get; set; }
        public string MonthName { get; set; }
        public string Abbrivation { get; set; }

    }
}
