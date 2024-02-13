namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Wf_WorkFlowDocumentPattern
    {
        public int ID { get; set; }

        public long WorkFlowDocumenTypetID { get; set; }

        public int ActionLevel { get; set; }

        public long IndividualID { get; set; }

        public int? RoleID { get; set; }

        [StringLength(50)]
        public string CreatedTerminal { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [StringLength(50)]
        public string ModifiedTerminal { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public string ApprovalHTMLEn { get; set; }

        public string ApprovalHTMLAr { get; set; }    
        public virtual Wf_WorkflowDocumentType Wf_WorkflowDocumentType { get; set; }
    }
}
