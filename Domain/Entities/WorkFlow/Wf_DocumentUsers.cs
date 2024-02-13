namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


 
    public partial class Wf_DocumentUsers
    {
        public long ID { get; set; }

        public long WorkFlowDocumentID { get; set; }

        public long IndividualID { get; set; }

        public int ActionLevel { get; set; }

        [StringLength(1)]
        public string Type { get; set; }

        [StringLength(50)]
        public string Remark { get; set; }

        public long? LastActionLog { get; set; }

        public bool? IsMarkCompleted { get; set; }

        public string ApprovalHTMLEn { get; set; }

        public string ApprovalHTMLAr { get; set; }      
        public virtual Wf_Documents Wf_Documents { get; set; }
    }
}
