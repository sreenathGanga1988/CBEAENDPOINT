using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public  class NewsItem : AuditEntity
    {
        public int Id { get; set; }
       
        public DateTime DateofAction { get; set; }

        public String NewsText { get; set; }

        public String NewsLink { get; set; }

       
    }
}
