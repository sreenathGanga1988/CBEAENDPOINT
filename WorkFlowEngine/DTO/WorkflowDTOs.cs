using System;
using System.Collections.Generic;
using System.Text;

namespace WorkFlowEngine.DTO
{
    public class WorkflowLogDTO
    {
        public long ID { get; set; }
        public String Name { get; set; }

        public String ActionType { get; set; }
        public int ActionLevel { get; set; }

        public String ActionTime { get; set; }
        public String Remark { get; set; }
    }
    public class DocumentPatternDTO
    {
        public long ID { get; set; }
        public long IndividualID { get; set; }

        public String ApprovalHTML { get; set; }
    }
    public class InboxItemDTO
    {
        public long ID { get; set; }
        public String Serial { get; set; }
        public String DocumentTypeID { get; set; }

        public String CreatedOn { get; set; }
        public String ActionDate { get; set; }
        public String DocumentTypeName { get; set; }
        public String CurrentLevel { get; set; }
        public string Userlevel { get; set; }

        public string Status { get; set; }
        public Boolean IsCompleted { get; set; }
        public Boolean? IsActive { get; set; }
        public bool IsShowApproveBtn { get; set; }
        public bool IsShowRejectBtn { get; set; }
        public bool IsShowViewBtn { get; set; }
        public string Actions { get; set; }
    }
    public class GridResultCollection<T> where T : class
    {
        /// <summary>
        /// Current page number (starts from 1)
        /// </summary>
        //public int page { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// Total number of records
        /// </summary>
        //public int records { get; set; }

        /// <summary>
        /// Result rows
        /// </summary>
        public List<T> rows { get; set; }

        public string error { get; set; }

        public int code { get; set; }
    }
}
