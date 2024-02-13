using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace DTO
{
    public class KiduTable
    {



        public List<String> ColumnHeaders { get; set; }
        public List<String> Columns { get; set; }

        public string IdColumn { get; set; }

        public string AddAction { get; set; }
        public string EditAction { get; set; }

        public string ViewAction { get; set; }

        public string DeleteAction { get; set; }

        public DataTable RowData { get; set; }

    }



    public class Audit_ViewModel
    {

        public Audit_ViewModel()
        {
            this.IsActive = true;
            this.IsDeleted = false;
        }

        public bool IsActive { get; set; }
        public int? CreatedByUserId { get; set; }
        public string AddedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedByUserId { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int? DeletedByByUserId { get; set; }
        public string DeletedUser { get; set; }
        public DateTime? DeletedDate { get; set; }


    }



    #region Category
    public class CategoryDTO : Audit_ViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }

    }

    public class CategoryDTOList : KiduTable
    {
        public CategoryDTOList()
        {
            this.ColumnHeaders = new List<string>() { "Category Name", "Code" };
            this.Columns = new List<string>() { "name", "abbreviation" };
            this.IdColumn = "id";
            this.AddAction = "/Category/Create";
            this.EditAction = "/Category/Edit";
            this.ViewAction = "/Category/Details";
            this.DeleteAction = "/Category/Delete";

            this.RowData = new DataTable();
            this.Categorys = new List<CategoryDTO>();

        }

        public List<CategoryDTO> Categorys { get; set; }
    }
    #endregion






    #region User Types

    public partial class UserTypeDTO
    {
        public int Id { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
    }

    public class UserTypeDTOList : KiduTable
    {
        public UserTypeDTOList()
        {
            this.ColumnHeaders = new List<string>() { "User Type", "Description" };
            this.Columns = new List<string>() { "abbreviation", "description" };
            this.IdColumn = "id";

            this.AddAction = "/UserType/Create";
            this.EditAction = "/UserType/Edit";
            this.ViewAction = "/UserType/Details";
            this.DeleteAction = "/UserType/Delete";

            this.RowData = new DataTable();
            this.UserTypes = new List<UserTypeDTO>();

        }

        public List<UserTypeDTO> UserTypes { get; set; }
    }
    #endregion




    #region User 

    public partial class UserDTO : Audit_ViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? UserTypeId { get; set; }
        public int? EmployeeId { get; set; }

        public DateTime? ValidTill { get; set; }
        public string UserStatus { get; set; }
        public string OldPassWord { get; set; }

        public string ConfirmPassword { get; set; }
        public string EmailId { get; set; }

        public string PhoneNum { get; set; }

       
        public string EmployeeName { get; set; }

       
        public string UserTypeName { get; set; }

    }

    public class UserDTOList : KiduTable
    {
        public UserDTOList()
        {
            this.ColumnHeaders = new List<string>() { "User Name", "UserType" };
            this.Columns = new List<string>() { "userName", "userTypeId" };
            this.IdColumn = "id";

            this.AddAction = "/User/Create";
            this.EditAction = "/User/Edit";
            this.ViewAction = "/User/Details";
            this.DeleteAction = "/User/Delete";

            this.RowData = new DataTable();
            this.Users = new List<UserDTO>();

        }

        public List<UserDTO> Users { get; set; }
    }
    #endregion




    public class StoresDTO
    {
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string Street { get; set; }
        public string Phone { get; set; }
        public string StoreTaxId { get; set; }
        public bool Isactive { get; set; }
    }

    public class StoresDTOList : KiduTable
    {
        public StoresDTOList()
        {
            this.ColumnHeaders = new List<string>() { "Store Name", "Phone", "Tax Id", "Active" };
            this.Columns = new List<string>() { "storeName", "phone", "StoreTaxId", "isactive" };
            this.IdColumn = "id";

            this.EditAction = "/Stores/Edit";
            this.ViewAction = "/Stores/Detail";
            this.DeleteAction = "/Stores/Delete";

            this.IdColumn = "StoreID";

            this.RowData = new DataTable();
            this.Stores = new List<StoresDTO>();

        }

        public List<StoresDTO> Stores { get; set; }
    }






    public class MainPageDTO
    {

        public MainPageDTO()
        {
           
            CorouselImage1 = @"/images/CC1.JPEG";
            CorouselImage2 = @"/images/CC2.JPEG";
            CorouselImage3 = @"/images/c3.jpg";
        }



        public int Id { get; set; }


        public String CorouselImage1 { get; set; }
        public String CorouselImage2 { get; set; }
        public String CorouselImage3 { get; set; }
        public String MainText { get; set; }
        public String Slogan { get; set; }
        public String RulesRegulation { get; set; }
        public String DayQuote { get; set; }

        public String LogoImage1 { get; set; }
        public String LogoImage2 { get; set; }

        public String ContactDesc1 { get; set; }

        public String ContactDesc2 { get; set; }

        public String ContactLine1 { get; set; }

        public String ContactLine2 { get; set; }

        public String ContactLine3 { get; set; }

        public String Phonenum { get; set; }

        public String Faxnum { get; set; }


        public String Website { get; set; }
        public String Email { get; set; }
    }

    public class MainPageDTOList : KiduTable
    {
        public MainPageDTOList()
        {
            this.ColumnHeaders = new List<string>() { "Slogan", "DayQuote", "Corousel Image1", "Corousel Image2", "Corousel Image3" };
            this.Columns = new List<string>() { "slogan", "dayQuote", "corouselImage1", "corouselImage2", "corouselImage3" };
            this.IdColumn = "id";

            this.EditAction = "/MainPage/Edit";
            this.ViewAction = "/MainPage/Detail";
            this.DeleteAction = "/MainPage/Delete";         

            this.RowData = new DataTable();
            this.MainPages = new List<MainPageDTO>();
        }

        public List<MainPageDTO> MainPages { get; set; }
    }







    public class NewsItemDTO : Audit_ViewModel
    {
        public int Id { get; set; }
        public DateTime DateofAction { get; set; }

        public String NewsText { get; set; }

        public String NewsLink { get; set; }
    }


    public class NewsItemDTOList : KiduTable
    {
        public NewsItemDTOList()
        {
            this.ColumnHeaders = new List<string>() {  "Date", "News", "News Link" };
            this.Columns = new List<string>() { "dateofAction", "newsText", "newsLink" };
            this.IdColumn = "newsID";

            this.EditAction = "/NewsItem/Edit";
            this.ViewAction = "/NewsItem/Detail";
            this.DeleteAction = "/NewsItem/Delete";

            this.RowData = new DataTable();
            this.newsitems = new List<NewsItemDTO>();
        }

        public List<NewsItemDTO> newsitems { get; set; }
    }









    public class StateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public Boolean IsActive { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedByUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class StateDTOList : KiduTable
    {
        public StateDTOList()
        {
            this.ColumnHeaders = new List<string>() { "Name", "Code", "IsActive ?" };
            this.Columns = new List<string>() { "name", "abbreviation", "isActive" };
            this.IdColumn = "id";

            this.AddAction = "/State/Create";
            this.EditAction = "/State/Edit";
            this.ViewAction = "/State/Details";
            this.DeleteAction = "/State/Delete";

            this.RowData = new DataTable();
            this.States = new List<StateDTO>();

        }

        public List<StateDTO> States { get; set; }
    }




    public class CircleDTO
    {


        public int Id { get; set; }
        public int? CircleCode { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public bool IsActive { get; set; }
        public int? StateId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedByUserId { get; set; }
        public DateTime ModifiedDate { get; set; }


    }
    public class CircleDTOList : KiduTable
    {
        public CircleDTOList()
        {
            this.ColumnHeaders = new List<string>() { "CircleCode", "Name", "Abbreviation", "IsActive" };
            this.Columns = new List<string>() { "circleCode", "name", "abbreviation", "isActive" };
            this.IdColumn = "id";

            this.AddAction = "/Circle/Create";
            this.EditAction = "/Circle/Edit";
            this.ViewAction = "/Circle/Details";
            this.DeleteAction = "/Circle/Delete";

            this.RowData = new DataTable();
            this.Circles = new List<CircleDTO>();

        }

        public List<CircleDTO> Circles { get; set; }
    }






    public partial class DesignationDTO
    {


        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedByUserId { get; set; }
        public DateTime ModifiedDate { get; set; }


    }


    public class DesignationDTOList : KiduTable
    {
        public DesignationDTOList()
        {
            this.ColumnHeaders = new List<string>() { "Name", "Description" };
            this.Columns = new List<string>() { "name", "description" };
            this.IdColumn = "id";

            this.AddAction = "/Designation/Create";
            this.EditAction = "/Designation/Edit";
            this.ViewAction = "/Designation/Details";
            this.DeleteAction = "/Designation/Delete";

            this.RowData = new DataTable();
            this.Designations = new List<DesignationDTO>();

        }

        public List<DesignationDTO> Designations { get; set; }
    }







    public class BranchDTO
    {


        public int Id { get; set; }
        public int DpCode { get; set; }
        public int CircleId { get; set; }
        public int? StateId { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string District { get; set; }
        public bool IsActive { get; set; }
        public bool IsRegCompleted { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedByUserId { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string Circle_text { get; set; }
        public string State_text { get; set; }
        public string Status { get; set; }
    }



    public class BranchDTOList : KiduTable
    {
        public BranchDTOList()
        {
            this.ColumnHeaders = new List<string>() { "DpCode", "Circle", "State", "Name", "District" };
            this.Columns = new List<string>() { "dpCode", "circle_text", "state_text", "name", "district" };
            this.IdColumn = "dpCode";

            this.AddAction = "/Branch/Create";
            this.EditAction = "/Branch/Edit";
            this.ViewAction = "/Branch/Details";
            this.DeleteAction = "/Branch/Delete";

            this.RowData = new DataTable();
            this.Branchs = new List<BranchDTO>();

        }

        public List<BranchDTO> Branchs { get; set; }
    }





    public class MemberDTO
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
    }



    public class MemberDTOList : KiduTable
    {
        public MemberDTOList()
        {
            this.ColumnHeaders = new List<string>() { "Staff No", "Name", "Gender", "Designation", "Category", "Status" };
            this.Columns = new List<string>() { "staffNo", "name", "gender_text", "designation_text", "category_text", "status_text" };
            this.IdColumn = "id";

            this.AddAction = "/Member/Create";
            this.EditAction = "/Member/Edit";
            this.ViewAction = "/Member/Details";
            this.DeleteAction = "/Member/Delete";

            this.RowData = new DataTable();
            this.Members = new List<MemberDTO>();

        }

        public List<MemberDTO> Members { get; set; }
    }

    public class AccountsDTO
    {
        public string Staffno { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }

        public Decimal Amount { get; set; }
    }


    public class ContributionlistDTO
    {
        public string Staffno { get; set; }
        public string Year { get; set; }
        public decimal Jan { get; set; }
        public decimal Feb { get; set; }
        public decimal Mar { get; set; }
        public decimal Apr { get; set; }
        public decimal May { get; set; }
        public decimal Jun { get; set; }
        public decimal July { get; set; }
        public decimal Aug { get; set; }
        public decimal Sep { get; set; }
        public decimal Oct { get; set; }
        public decimal Nov { get; set; }
        public decimal Dec { get; set; }

        public decimal Total { get; set; }
    }








    public partial class AccountsDirectEntryDTO : Audit_ViewModel
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


    public class AccountsDirectEntryDTOList : KiduTable
    {
        public AccountsDirectEntryDTOList()
        {
            this.ColumnHeaders = new List<string>() { "Staff No", "Name", "DpCode", "DdIbaDate", "Amt"};
            this.Columns = new List<string>() { "staffNo", "name", "dpCode", "ddIbaDate", "amt" };
            this.IdColumn = "id";

            this.AddAction = "/AccountsDirectEntry/Create";
            this.EditAction = "/AccountsDirectEntry/Edit";
            this.ViewAction = "/AccountsDirectEntry/Details";
            this.DeleteAction = "/AccountsDirectEntry/Delete";

            this.RowData = new DataTable();
            this.AccountsDirectEntrys = new List<AccountsDirectEntryDTO>();

        }

        public List<AccountsDirectEntryDTO> AccountsDirectEntrys { get; set; }
    }


    public partial class DayQuoteDTO
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public int MonthCode { get; set; }
        public String ToDayQuote { get; set; }
        public String UnformatedContent { get; set; }

        public int CreatedByUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedByUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }

    public class DayQuoteDTOList : KiduTable
    {
        public DayQuoteDTOList()
        {
            this.ColumnHeaders = new List<string>() { "Day", "Month", "ToDayQuote"};
            this.Columns = new List<string>() { "day", "monthCode", "toDayQuote" };
            this.IdColumn = "Id";

            this.AddAction = "/DayQuote/Create";
            this.EditAction = "/DayQuote/Edit";
            this.ViewAction = "/DayQuote/Details";
            this.DeleteAction = "/DayQuote/Delete";

            this.RowData = new DataTable();
            this.DayQuotes = new List<DayQuoteDTO>();

        }

        public List<DayQuoteDTO> DayQuotes { get; set; }
    }

    
     public class MainReportDTO : Audit_ViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public String SQLString { get; set; }
        

    }
    public class MainReportDTOList : KiduTable
    {
        public MainReportDTOList()
        {
            this.ColumnHeaders = new List<string>() { "Name", "Description", "SQLString" };
            this.Columns = new List<string>() { "name", "description", "sqlString" };
            this.IdColumn = "Id";

            this.AddAction = "/MainReport/Create";
            this.EditAction = "/MainReport/Edit";
            this.ViewAction = "/MainReport/Details";
            this.DeleteAction = "/MainReport/Delete";

            this.RowData = new DataTable();
            this.MainReports = new List<MainReportDTO>();

        }

        public List<MainReportDTO> MainReports { get; set; }
    }


    public class DeathClaimDTOList : KiduTable
    {
        public DeathClaimDTOList()
        {
            this.ColumnHeaders = new List<string>() { "StaffNo", "Staff Name", "Designation",  "DDDATE", "Amount" };
            this.Columns = new List<string>() { "staffNo", "staffName", "branchName", "dddate", "amount" };
            this.IdColumn = "Id";

            this.AddAction = "/DeathClaim/Create";
            this.EditAction = "/DeathClaim/Edit";
            this.ViewAction = "/DeathClaim/Details";
            this.DeleteAction = "/DeathClaim/Delete";

            this.RowData = new DataTable();
            this.DeathClaimDTOs = new List<DeathClaimDTO>();

        }

        public List<DeathClaimDTO> DeathClaimDTOs { get; set; }
    }



    public class DeathClaimDTO
    {
        
        public int Id { get; set; }
        public int StaffNo { get; set; }
        public int DpCode { get; set; }
        public int? StateId { get; set; }
        public int? DesignationId { get; set; }
        public DateTime? DeathDate { get; set; }
        public string Nominee { get; set; }
        public string NomineeRelation { get; set; }
        public string NomineeIDentity { get; set; }
        public int YearOF { get; set; }
        public string DDNO { get; set; }
        public DateTime? DDDATE { get; set; }
        public decimal Amount { get; set; }
        public float LastContribution { get; set; }
        public string StaffName { get; set; }
        public string BranchName { get; set; }
        public string StateName { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedByUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }


    public class SupportTicketDTO
    {
        public int Id { get; set; }

        public String SupportTicketNum { get; set; }

        public String Description { get; set; }

        public String Priority { get; set; }

        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }


        public String Duration { get; set; }

        public String DeveloperRemark { get; set; }

        public Boolean isApproved { get; set; }

        public int? ApprovedByUserId { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }

    public class SupportTicketDTOList : KiduTable
    {
        public SupportTicketDTOList()
        {
            this.ColumnHeaders = new List<string>() { "SupportTicketNum", "Description", "Priority", "Duration", "DeveloperRemark" };
            this.Columns = new List<string>() { "supportTicketNum", "description", "priority", "duration", "developerRemark" };
            this.IdColumn = "Id";

            this.AddAction = "/SupportTicket/Create";
            this.EditAction = "/SupportTicket/Edit";
            this.ViewAction = "/SupportTicket/Details";
            this.DeleteAction = "/SupportTicket/Delete";

            this.RowData = new DataTable();
            this.SupportTickets = new List<SupportTicketDTO>();

        }

        public List<SupportTicketDTO> SupportTickets { get; set; }
    }
    public  class FileUploadDetailDTO
    {

        public int Id { get; set; }

        public string Filename { get; set; }

        public string FileDesription { get; set; }

        public string FileType { get; set; }

        public string FileLocation { get; set; }

        public Boolean isPublic { get; set; }

        public String FileRelativeLocation { get; set; }

        public Byte[] FileContent { get; set; }
    }

    public class FileUploadDetailDTOList : KiduTable
    {
        public FileUploadDetailDTOList()
        {
            this.ColumnHeaders = new List<string>() { "Filename", "FileDesription", "FileType", "isPublic" };
            this.Columns = new List<string>() { "filename", "fileDesription", "fileType", "isPublic" };
            this.IdColumn = "Id";

            this.AddAction = "/FileUploadDetail/Create";
            this.EditAction = "/FileUploadDetail/Edit";
            this.ViewAction = "/FileUploadDetail/Details";
            this.DeleteAction = "/FileUploadDetail/Delete";

            this.RowData = new DataTable();
            this.FileUploadDetails = new List<FileUploadDetailDTO>();

        }

        public List<FileUploadDetailDTO> FileUploadDetails { get; set; }
    }






    public class YearMasterDTO
    {
       
        public int YearOf { get; set; }
    }

    public class YearMasterDTOList : KiduTable
    {
        public YearMasterDTOList()
        {
            this.ColumnHeaders = new List<string>() { "YearOf" };
            this.Columns = new List<string>() { "yearOf"};
            this.IdColumn = "Id";

            this.AddAction = "/YearMaster/Create";
            this.EditAction = "/YearMaster/Edit";
            this.ViewAction = "/YearMaster/Details";
            this.DeleteAction = "/YearMaster/Delete";

            this.RowData = new DataTable();
            this.YearMasters = new List<YearMasterDTO>();

        }

        public List<YearMasterDTO> YearMasters { get; set; }
    }


    public class ClaimCountDTO : Audit_ViewModel
    {
        public int Id { get; set; }
        public int StateId { get; set; }

        public int YearOF { get; set; }


        public int DesignationId { get; set; }

        public int Count { get; set; }

    }


    public class ClaimCountDTOList : KiduTable
    {
        public ClaimCountDTOList()
        {
            this.ColumnHeaders = new List<string>() { "StateId","YearOF", "DesignationId", "Count" };
            this.Columns = new List<string>() { "stateId","yearOF", "designationId", "count" };
            this.IdColumn = "Id";

            this.AddAction = "/ClaimCount/Create";
            this.EditAction = "/ClaimCount/Edit";
            this.ViewAction = "/ClaimCount/Details";
            this.DeleteAction = "/ClaimCount/Delete";

            this.RowData = new DataTable();
            this.ClaimCountDTOS = new List<ClaimCountDTO>();

        }

        public List<ClaimCountDTO> ClaimCountDTOS { get; set; }
    }




    public class ManagingComiteeDTO :Audit_ViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public string Description1 { get; set; }


        public string Description2 { get; set; }

        public string imageLocation { get; set; }

        public int order { get; set; }
    }


    public class ManagingComiteeDTOList : KiduTable
    {
        public ManagingComiteeDTOList()
        {
            this.ColumnHeaders = new List<string>() { "Name", "Position", "Description1", "Description2", "imageLocation" , "order" };
            this.Columns = new List<string>() { "name", "position", "description1", "description2", "imageLocation", "order" };
            this.IdColumn = "Id";

            this.AddAction = "/ManagingComitee/Create";
            this.EditAction = "/ManagingComitee/Edit";
            this.ViewAction = "/ManagingComitee/Details";
            this.DeleteAction = "/ManagingComitee/Delete";

            this.RowData = new DataTable();
            this.ManagingComitees = new List<ManagingComiteeDTO>();

        }

        public List<ManagingComiteeDTO> ManagingComitees { get; set; }
    }
    public class ReportParameter
    {
        public String ReportType { get; set; }

        public String StaffNO { get; set; }

        public int YearOf { get; set; }

        public int MonthCode { get; set; }

        public int DpCode { get; set; }

        public int CircleId { get; set; }

        public String ids { get; set; }
    }
    public class ReportDTO : ReportParameter
    {

        public ReportDTO()
        {
            dataTables = new List<DataTable>();
        }

        public List<DataTable> dataTables { get; set; }
    }

    public class claimdata
    {
       
        public DataTable dt1 { get; set; }
        public DataTable dt2 { get; set; }
    }
    public class ApproveDTO
    {
        public int ID { get; set; }
        public int UseriD { get; set; }
        public Boolean approve { get; set; }

        public string type { get; set; }
    }
public class CustomErrorLogDTO : Audit_ViewModel
    {

        public String Detail { get; set; }
        public String log { get; set; }
        public String Reason { get; set; }

        public String Remark { get; set; }

        public String Reference { get; set; }

    }
   
}


  


