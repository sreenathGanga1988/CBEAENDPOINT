using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class YearMaster
    {
        [Key]
        public int YearOf { get; set; }

        public int YearName { get; set; }

    }



}
