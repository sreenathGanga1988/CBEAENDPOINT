using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
  public  class ProfileDTO
    {

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


        public string Gender_text { get; set; }

        public string Designation_text { get; set; }

        public string Category_text { get; set; }

        public string Status_text { get; set; }
//test
      public   List<ContributionlistDTO> contributionlistDTO { get; set; }
    }
}
