using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace DTO
{

 
    public class FileDetails
    {

        public String FileName { get; set; }
        public String FileLocation { get; set; }

        public String FileType { get; set; }

        public String FileExtension { get; set; }

        public Decimal  FileSize { get; set; }

    }

    public class ContributionMasterDTO : FileDetails
    {

        public ContributionMasterDTO()
        {
            ContributionDetails = new List<ContributionDetailDTO>();
            WrongLines = new List<string>();
            this.FileType = "Contribution";
            customErrorLogDTOs = new List<CustomErrorLogDTO>();
        }


        public long Id { get; set; }
      


        public int Month { get; set; }
        public int Year { get; set; }

        public string Circle { get; set; }

        public String totalamount { get; set; }
        public String totalentry { get; set; }

        public String NewMemberCount { get; set; }

        public Boolean isApproved { get; set; }
        
        public String ContributionStatus { get; set; }
        public String AddedBy { get; set; }
        public String AddedDate { get; set; }


        public String ApprovedBy { get; set; }
        public String ApprovedDate { get; set; }

        public  List<String> WrongLines { get; set; }
        public List<ContributionDetailDTO> ContributionDetails { get; set; }


        public List<CustomErrorLogDTO> customErrorLogDTOs { get; set; }
        public void ReadContributionFromFile()
        {
            IEnumerable<String> alllines = File.ReadLines(this.FileLocation);

          

            int totalamount = 0;
            int totalentry = 0;
            foreach (var item in alllines)
            {
                try
                {

                    if(item.Length>= 75)
                    {

                        

                        ContributionDetailDTO detail = new ContributionDetailDTO();
                        detail.Circle = item.Substring(0, 5);
                        detail.Month = int.Parse ( item.Substring(5, 2));
                        detail.Year = int.Parse(item.Substring(7, 4));
                        detail.DpCode = item.Substring(11, 5);
                        detail.StaffNo = item.Substring(16, 6);
                        detail.Name = item.Substring(22, 31);
                        detail.Designation = item.Substring(53, 15);
                        detail.Contribution = int.Parse(item.Substring(68, 7).ToString()) / 100;
                        detail.FullString = item;
                        if (detail.Year==this.Year && detail.Month ==this.Month)
                        {
                            totalentry++;
                            totalamount = totalamount + detail.Contribution;
                            this.ContributionDetails.Add(detail);
                        }
                        else
                        {
                            customErrorLogDTOs.Add(new CustomErrorLogDTO() { Detail = item, log = "Wrong year",Remark="Bankfile" });
                            WrongLines.Add(item  +"-----/Wrong Period");
                        }
                           
                    }
                    else
                    {
                        customErrorLogDTOs.Add(new CustomErrorLogDTO() { Detail = item, log = "Wrong Length", Remark = "Bankfile" });
                       
                    }

                    

                  
                }
                catch (Exception ex)
                {
                    customErrorLogDTOs.Add(new CustomErrorLogDTO() { Detail = item, log = "Error in reading", Remark = "Bankfile" });                  
                   
                }
               
            }

            this.totalamount = totalamount.ToString();
            this.totalentry = totalentry.ToString();

            
            

        }


        public string FilenameGenerator()
        {
            String filename = "";

            filename = this.FileType  + "_" + this.Year + "_" + this.Month + "_" + this.Circle + this.FileExtension ;

            return filename;

        }





    }
    public class ContributionDetailDTO
    {
        public String FullString { get; set; }
        public String Circle { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public String DpCode { get; set; }
        public String StaffNo { get; set; }
        public String Name { get; set; }
        public String Designation { get; set; }
        public int Contribution { get; set; }
        public String Total { get; set; }

        public long Id { get; set; }
        public Decimal Amount { get; set; }
        public Boolean isParked { get; set; }
        public long ContributionMasterId { get; set; }
        public String ParkReason { get; set; } = "";
        public DateTime? Parkedon { get; set; }

        public DateTime? UnParkedon { get; set; }

    }


    public class ContributionMasterDTOList : KiduTable
    {
        public ContributionMasterDTOList()
        {
            this.ColumnHeaders = new List<string>() { "ID", "Name", "Position", "Description1", "Description2", "imageLocation", "order" };
            this.Columns = new List<string>() { "id", "name", "position", "description1", "description2", "imageLocation", "order" };
            this.IdColumn = "Id";

            this.AddAction = "/ManagingComitee/Create";
            this.EditAction = "/ManagingComitee/Edit";
            this.ViewAction = "/ManagingComitee/Details";
            this.DeleteAction = "/ManagingComitee/Delete";

            this.RowData = new DataTable();
            this.ContributionMasterDTOs = new List<ContributionMasterDTO>();

        }

        public List<ContributionMasterDTO> ContributionMasterDTOs  { get; set; }
    }
}
