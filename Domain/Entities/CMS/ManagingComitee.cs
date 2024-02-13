using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ManagingComitee :AuditEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public string Description1 { get; set; }


        public string Description2 { get; set; }

        public string imageLocation { get; set; }

        public int order { get; set; }
    }
}
