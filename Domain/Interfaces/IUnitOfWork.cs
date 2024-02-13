using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {




      
        IAccountsRepository AccountsRepository { get; }
     
        IAccountsDirectEntryRepository AccountsDirectEntry { get; }
        
        IBranchRepository Branch { get; }
        ICategoryRepository Category { get; }
   
        ICircleRepository Circle { get; }
        ICircleStateRepository CircleState { get; }
        IDesignationRepository Designation { get; }
        IImageRepository Image { get; }
        IMemberRepository Member { get; }
        IMonthRepository Month { get; }
        IStateRepository State { get; }
        IStatusRepository Status { get; }
        ITransferRepository Transfer { get; }
        IUserRepository User { get; }
        IUserTypeRepository UserType { get; }
        IEmployeeProfileRepository EmployeeProfileRepository { get; }

        IMainPageRepository MainPage { get; }

        INewsItemRepository NewsItem { get; }

        IDayQuoteRepository DayQuote { get; }
        ISupportTicketRepository SupportTicket { get; }

         IFileUploadRepository FileUploadDetail { get; }

        IRefundRepository RefundRepository { get; }
        IDeathClaimRepository DeathClaimRepository { get; }
        IYearMasterRepository YearMaster { get; }

        IClaimCountRepository ClaimCount { get; }

        IManagingComiteeRepository ManagingComitee { get; }

        IReportRepository ReportRepository  { get; }

        IContributionMasterRepository Contributions { get; }

        IContributionDetailRepository ContributionDetails { get; }

        IMainReportRepository MainReport { get; }

        ICustomErrorLogRepository CustomErrorLogRepository { get; }





        IWorkflowDocumentTypeRepository WorkflowDocumentTypeRepository { get; }
        IWorkFlowDocumentPatternRepository WorkFlowDocumentPatternRepository { get; }
        IWokflowDocumentsRepository WokflowDocumentsRepository { get; }
        IWokflowDocumentLogRepository WokflowDocumentLogRepository { get; }
        IWokflowDocumentUsersRepository WokflowDocumentUsersRepository { get; }

        IAppAuditRepository AppAuditRepository { get; }






        int SaveAllChanges();
    }
}
