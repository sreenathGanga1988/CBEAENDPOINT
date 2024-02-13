using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class Member
    {
        public Member()
        {
            Accounts = new HashSet<Accounts>();
            Transfer = new HashSet<Transfer>();
        }

        public int Id { get; set; }
        public int StaffNo { get; set; }
        public string Name { get; set; }
        public int GenderId { get; set; }
        public int? ImageId { get; set; }
        public DateTime? Dob { get; set; }
        public int? DesignationId { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? Doj { get; set; }
        public int? DpCode { get; set; }
        public DateTime? DojtoScheme { get; set; }
        public int StatusId { get; set; }
        public bool IsRegCompleted { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedByUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string Nominee { get; set; }

        public string NomineeRelation { get; set; }

        public string NomineeIDentity { get; set; }


        public string UnionMember { get; set; }

        public virtual Category Category { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual Designation Designation { get; set; }
        public virtual Branch DpCodeNavigation { get; set; }
        public virtual Image Image { get; set; }
        public virtual User ModifiedByUser { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<Accounts> Accounts { get; set; }
        public virtual ICollection<Transfer> Transfer { get; set; }

        [NotMapped]
        public string Gender_text { get; set; }
        [NotMapped]
        public string Designation_text { get; set; }
        [NotMapped]
        public string Category_text { get; set; }
        [NotMapped]
        public string Status_text { get; set; }
        [NotMapped]
        public string TotalContribution { get; set; }

        [NotMapped]
        public string TotalRefund { get; set; }

    }
}
