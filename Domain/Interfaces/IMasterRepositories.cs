using Domain.Entities;
using Domain.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Domain.Interfaces
{







  
   
    public interface IAccountsRepository : IGenericRepository<Accounts>
    {
        List<dynamic> GetContribution(int staffno);
        List<dynamic> GetContribution(int Year = 0, int monthcode = 0, int DpCode = 0, int circleCode = 0, int staffno = 0);
    }
       public interface IAccountsDirectEntryRepository : IGenericRepository<AccountsDirectEntry> {
        Boolean Approve(int ID, int UseriD, Boolean approve);
    }
     public interface IBranchRepository : IGenericRepository<Branch>
    {

        List<Branch> GetBrachDetails();
        Branch GetBrachDetail(int id);
        List<dynamic> GetSelectList(Expression<Func<Branch, bool>> expression, int skip = 0, int nop = 0);
        IQueryable<Branch> GetAllBranchasync(Expression<Func<Branch, bool>> expression);


    }
    public interface ICategoryRepository : IGenericRepository<Category> {

        List<dynamic> GetSelectList(Expression<Func<Category, bool>> expression, int skip = 0, int nop = 0);


    }
  
    public interface ICircleRepository : IGenericRepository<Circle>
    {

        List<dynamic> GetSelectList(Expression<Func<Circle, bool>> expression, int skip = 0, int nop = 0);

    }
    public interface ICircleStateRepository : IGenericRepository<CircleState> { }
    public interface IDesignationRepository : IGenericRepository<Designation> {
        List<dynamic> GetSelectList(Expression<Func<Designation, bool>> expression, int skip = 0, int nop = 0);
    }
    public interface IImageRepository : IGenericRepository<Image> { }
    public interface IMemberRepository : IGenericRepository<Member>
    {
        List<string> GetAllStaff();
        Member GetemployeeProfile(int id);
        List<Member> GetAllemployeeProfile();
        List<dynamic> GetSelectList(Expression<Func<Member, bool>> expression, int skip = 0, int nop = 0);
        IQueryable<Member> GetAllMemberasync(Expression<Func<Member, bool>> expression);
    }
      public interface IMonthRepository : IGenericRepository<Month>
    {
        List<dynamic> GetSelectList(Expression<Func<Month, bool>> expression, int skip = 0, int nop = 0);

    }
    
    public interface IStateRepository : IGenericRepository<State> {

        List<dynamic> GetSelectList(Expression<Func<State, bool>> expression, int skip = 0, int nop = 0);
    }
    public interface IStatusRepository : IGenericRepository<Status> {
        List<dynamic> GetSelectList(Expression<Func<Status, bool>> expression, int skip = 0, int nop = 0);
    }
    public interface ITransferRepository : IGenericRepository<Transfer> { }
    public interface IUserRepository : IGenericRepository<User> {

        User GetvalidUser(string UserName, string Password, out string message);
        IQueryable<User> GetAllMemberasync(Expression<Func<User, bool>> expression);
        Boolean Approve(int ID, int UseriD, Boolean approve);
       
    }
       public interface IUserTypeRepository : IGenericRepository<UserType> {
        List<dynamic> GetSelectList(Expression<Func<UserType, bool>> expression, int skip = 0, int nop = 0);
    }
    public interface IMainPageRepository : IGenericRepository<MainPage> { }
    public interface INewsItemRepository : IGenericRepository<NewsItem> { }

    public interface IDayQuoteRepository : IGenericRepository<DayQuote> { }

    public interface ISupportTicketRepository : IGenericRepository<SupportTicket> { }



    public interface IFileUploadRepository : IGenericRepository<FileUploadDetail> {

        List<dynamic> GetSelectList(Expression<Func<FileUploadDetail, bool>> expression, int skip = 0, int nop = 0);
    }
    public interface IYearMasterRepository : IGenericRepository<YearMaster> {

        List<dynamic> GetSelectList(Expression<Func<YearMaster, bool>> expression, int skip = 0, int nop = 0);
    }
    public interface IMainReportRepository : IGenericRepository<MainReport>
    {

        List<dynamic> GetSelectList(Expression<Func<MainReport, bool>> expression, int skip = 0, int nop = 0);
    }



    
    public interface IClaimCountRepository : IGenericRepository<ClaimCount> {
        List<ClaimCount> GetclaimCount();
    }
    public interface IManagingComiteeRepository : IGenericRepository<ManagingComitee>
    {
      
    }

    public interface IDeathClaimRepository : IGenericRepository<DeathClaim>
    {
        List<dynamic> GetAllClaims();

    }
    public interface IRefundRepository : IGenericRepository<RefundContribution>
    {
        List<dynamic> GetAllRefunds();

    }
    public interface IContributionMasterRepository : IGenericRepository<ContributionMaster>
    {
        List<dynamic> GetContribution(int contributionID,int isapproved);
        List<dynamic> GetAllContribution();
        Boolean Approve(int ID, int UseriD, Boolean approve);

        DataTable GetTotalContribution();

        DataTable GetMemberDefaults();

        //void InsertContribution(ContributionMaster contributionMaster,List<ContributionDetail>)
    }

    public interface IContributionDetailRepository : IGenericRepository<ContributionDetail>
    {
        int GetWrongMembers(int contributionID);
        int GetWrongDpCode(int contributionID);

        int GetWrongCircle(int contributionID);



        Boolean UnParkContribution(int ContributiondetailId, int UseriD, out string Message);
        List<Accounts> GetAccounts(int contributionID);
    }
  public interface ICustomErrorLogRepository : IGenericRepository<CustomErrorLog>
    {      
    }
    public interface IEmployeeProfileRepository
    {
        List<dynamic> GetemployeeProfile(int id);
    }
    public interface IReportRepository
    {
        List<dynamic> GetReport(ReportParameterlist reportParameterlist);
        DataTable GetDataReport(String ReportType, string id);

        DataTable GetDataReport(String sqlstring);
        DataSet GetDataSetReport(String sqlstring)

    }

public interface IAppAuditRepository : IGenericRepository<App_EntityLog>
    {
        void AddLogs(String EntityName, Object PrimaryKey, String ActionType, String CreatedByUserId, Object objdata);


    }


}