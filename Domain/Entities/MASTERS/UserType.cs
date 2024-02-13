using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class UserType 
    {
        public int Id { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
    }
}
