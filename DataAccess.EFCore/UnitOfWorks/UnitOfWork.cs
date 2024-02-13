using DataAccess.EFCore.Repositories;
using Domain.Interfaces;
using System;
using System.Data;

namespace DataAccess.EFCore.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        private readonly IDbConnection _dbConnection;

        public UnitOfWork(ApplicationContext context, IDbConnection dbConnection)
        {
            _context = context;

            _dbConnection = dbConnection;

            AccountsRepository = new AccountsRepository(_context);
                      AccountsDirectEntry = new AccountsDirectEntryRepository(_context);
             Branch = new BranchRepository(_context);
            Category = new CategoryRepository(_context);

            Circle = new CircleRepository(_context);
            CircleState = new CircleStateRepository(_context);
            Designation = new DesignationRepository(_context);
            Image = new ImageRepository(_context);
            Member = new MemberRepository(_context);
       
            Month = new MonthRepository(_context);
            
            State = new StateRepository(_context);
            Status = new StatusRepository(_context);
            Transfer = new TransferRepository(_context);
            User = new UserRepository(_context);
            UserType = new UserTypeRepository(_context);

            EmployeeProfileRepository = new EmployeeProfileRepository(_context);
            MainPage = new MainPageRepository(_context);
            NewsItem = new NewsItemRepository(_context);

            DayQuote = new DayQuoteRepository(_context);
            SupportTicket = new SupportTicketRepository(_context);

            FileUploadDetail = new FileUploadRepository(_context);

            YearMaster = new YearMasterRepository(_context);

            ClaimCount = new ClaimCountRepository(_context);

            ManagingComitee = new ManagingComiteeRepository(_context);

            ReportRepository = new ReportRepository(_context, _dbConnection);
            ContributionDetails = new ContributionDetailRepository(_context, _dbConnection);

            Contributions = new ContributionMasterRepository(_context, _dbConnection);

            MainReport = new MainReports(_context);
            CustomErrorLogRepository = new CustomErrorLogRepository(_context);
            RefundRepository= new RefundRepository(_context);
            DeathClaimRepository = new DeathClaimRepository(_context);
            WorkflowDocumentTypeRepository = new WorkflowDocumentTypeRepository(_context);
            WorkFlowDocumentPatternRepository = new WorkFlowDocumentPatternRepository(_context);
            WokflowDocumentsRepository = new WokflowDocumentsRepository(_context);
            WokflowDocumentLogRepository = new WokflowDocumentLogRepository(_context);
            WokflowDocumentUsersRepository = new WokflowDocumentUsersRepository(_context);
            AppAuditRepository = new AppAuditRepository(_context);
        }

        public IAccountsRepository AccountsRepository { get; private set; }
       
        public IAccountsDirectEntryRepository AccountsDirectEntry { get; private set; }
     
        public IBranchRepository Branch { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public ICircleRepository Circle { get; private set; }
        public ICircleStateRepository CircleState { get; private set; }
        public IDesignationRepository Designation { get; private set; }
        public IImageRepository Image { get; private set; }
        public IMemberRepository Member { get; private set; }
             public IMonthRepository Month { get; private set; }
          public IStateRepository State { get; private set; }
        public IStatusRepository Status { get; private set; }
        public ITransferRepository Transfer { get; private set; }
        public IUserRepository User { get; private set; }

        public IUserTypeRepository UserType { get; private set; }
        public IEmployeeProfileRepository EmployeeProfileRepository { get; private set; }

        public ISupportTicketRepository SupportTicket { get; private set; }
        public IMainPageRepository MainPage { get; private set; }

        public INewsItemRepository NewsItem { get; private set; }

        public IDayQuoteRepository DayQuote { get; private set; }

        public IFileUploadRepository FileUploadDetail { get; private set; }

        public IYearMasterRepository YearMaster { get; private set; }
        public IClaimCountRepository ClaimCount { get; private set; }

        public IManagingComiteeRepository ManagingComitee { get; private set; }

        public IReportRepository ReportRepository { get; private set; }

        public IContributionMasterRepository Contributions { get; private set; }

        public IContributionDetailRepository ContributionDetails { get; private set; }

        public IMainReportRepository MainReport { get; private set; }
        public ICustomErrorLogRepository CustomErrorLogRepository { get; private set; }

        public IDeathClaimRepository DeathClaimRepository { get; private set; }
        public IRefundRepository RefundRepository { get; private set; }
        public IWorkflowDocumentTypeRepository WorkflowDocumentTypeRepository { get; private set; }
        public IWorkFlowDocumentPatternRepository WorkFlowDocumentPatternRepository { get; private set; }
        public IWokflowDocumentsRepository WokflowDocumentsRepository { get; private set; }
        public IWokflowDocumentLogRepository WokflowDocumentLogRepository { get; private set; }
        public IWokflowDocumentUsersRepository WokflowDocumentUsersRepository { get; private set; }
        public IAppAuditRepository AppAuditRepository { get; private set; }


       
       
        public int SaveAllChanges()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}