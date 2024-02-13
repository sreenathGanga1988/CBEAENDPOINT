using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public  class UserRoleRight : AuditEntity
    {
        public int Id { get; set; }
       
        public DateTime ControllerName { get; set; }

        public String ActionName { get; set; }

        public int UserTypeID { get; set; }

        

       
    }
}
