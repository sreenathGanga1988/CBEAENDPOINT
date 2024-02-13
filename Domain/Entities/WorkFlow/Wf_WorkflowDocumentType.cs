namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    
    public partial class Wf_WorkflowDocumentType
    {
        public Wf_WorkflowDocumentType()
        {
            Wf_Documents = new HashSet<Wf_Documents>();
            Wf_WorkFlowDocumentPattern = new HashSet<Wf_WorkFlowDocumentPattern>();
        }

        public long ID { get; set; }

        public int? ServiceTypeDocumentID { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        public bool IsHaveApproval { get; set; }

        public int? ApprovalLevel { get; set; }

        public bool IsActive { get; set; }

        public bool IsInbox { get; set; }


        public bool IsShowApproveBtn { get; set; }
        public bool IsShowRejectBtn { get; set; }
        public bool IsShowViewBtn { get; set; }
        public String HtmlTemplate { get; set; }

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

        public long? ChildWorkFlowID { get; set; }     

        public virtual ICollection<Wf_Documents> Wf_Documents { get; set; }

        public virtual ICollection<Wf_WorkFlowDocumentPattern> Wf_WorkFlowDocumentPattern { get; set; }
    }
}
