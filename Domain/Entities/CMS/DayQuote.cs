using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public partial class DayQuote :AuditEntity
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public int MonthCode { get; set; }
        public String ToDayQuote { get; set; }
        public String UnformatedContent { get; set; }

       

    }
}
