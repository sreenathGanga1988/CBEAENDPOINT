namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    
    public partial class Wf_Documents
    {
        public Wf_Documents()
        {
            Wf_DocumentLog = new HashSet<Wf_DocumentLog>();
            Wf_DocumentUsers = new HashSet<Wf_DocumentUsers>();
        }

        public long ID { get; set; }

        [Required]
        [StringLength(20)]
        public string Serial { get; set; }

        public long WorkFlowDocumentTypeID { get; set; }

        public int CurrentLevel { get; set; }

        public bool IsCompleted { get; set; }
        public bool IsActive { get; set; }

        
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

        [StringLength(10)]
        public string Status { get; set; }

        public String HtmlTemplate { get; set; }

        public long? ParentDocumentID { get; set; }


        public virtual ICollection<Wf_DocumentLog> Wf_DocumentLog { get; set; }

        public virtual Wf_WorkflowDocumentType Wf_WorkflowDocumentType { get; set; }
        public virtual ICollection<Wf_DocumentUsers> Wf_DocumentUsers { get; set; }
    }
}
