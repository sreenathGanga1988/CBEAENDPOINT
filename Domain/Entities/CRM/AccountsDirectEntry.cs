using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class AccountsDirectEntry :AuditEntity
    {
        public int ID { get; set; }
        public double? StaffNo { get; set; }
        public string Name { get; set; }
        public double? DpCode { get; set; }

        public int MonthCode { get; set; }
        public int YearOf { get; set; }
        public string DdIba { get; set; }
        public string DdIbaDate { get; set; }
        public double? Amt { get; set; }
        public string Enrl { get; set; }
        public string Fine { get; set; }
        public string F9 { get; set; }
        public string F10 { get; set; }
        public string F11 { get; set; }

        public string status { get; set; }
        public Boolean isApproved { get; set; }
        public String ApprovedBy { get; set; }
        public String ApprovedDate { get; set; }
    }
}
