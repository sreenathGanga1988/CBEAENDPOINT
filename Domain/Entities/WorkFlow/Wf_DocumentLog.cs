namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

 
    public partial class Wf_DocumentLog
    {
        public long ID { get; set; }

        public long? WorkFlowDocumentID { get; set; }

        [StringLength(12)]
        public string ActionType { get; set; }

        public DateTime? ActionTime { get; set; }

        public long? IndividualID { get; set; }

        public string Remark { get; set; }

        public virtual Wf_Documents Wf_Documents { get; set; }
    }
}
