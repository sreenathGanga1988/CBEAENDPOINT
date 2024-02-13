using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public partial class MainReport : AuditEntity
    {
        public MainReport()
        {
          
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public String SQLString { get; set; }
    }
}
