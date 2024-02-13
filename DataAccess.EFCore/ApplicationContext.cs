using Domain;
using Domain.Entities;
using Domain.Entities.db_views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.EFCore
{
    public partial class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }


     
             public virtual DbSet<Accounts> Accounts { get; set; }

        public virtual DbSet<AccountsDirectEntry> AccountsDirectEntry { get; set; }
       
        public virtual DbSet<Branch> Branch { get; set; }
        public virtual DbSet<Category> Category { get; set; }
    
        public virtual DbSet<Circle> Circle { get; set; }
        public virtual DbSet<CircleState> CircleState { get; set; }
        public virtual DbSet<Designation> Designation { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<Member> Member { get; set; }
               public virtual DbSet<Month> Month { get; set; }
       
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Transfer> Transfer { get; set; }
        public virtual DbSet<User> User { get; set; }
     
        public virtual DbSet<UserType> UserType { get; set; }

        public virtual DbSet<MainPage> MainPages { get; set; }

        public virtual DbSet<NewsItem> NewsItems { get; set; }

        public virtual DbSet<DayQuote> DayQuotes { get; set; }

        public virtual DbSet<SupportTicket> SupportTickets { get; set; }

        public virtual DbSet<FileUploadDetail> FileUploads { get; set; }

        public virtual DbSet<ClaimCount> ClaimCounts { get; set; }
        public virtual DbSet<YearMaster> YearMasters { get; set; }

        public virtual DbSet<ManagingComitee> ManagingComitees { get; set; }


        public virtual DbSet<MemberDefault> MemberDefaults { get; set; }


        public virtual DbSet<ContributionMaster> ContributionMasters { get; set; }


        public virtual DbSet<ContributionDetail> ContributionDetails { get; set; }


        public virtual DbSet<MainReport> MainReports { get; set; }

        public virtual DbSet<CustomErrorLog> CustomErrorLogs { get; set; }

        public virtual DbSet<MemberDefaultTemp> MemberDefaultTemp { get; set; }

        public virtual DbSet<DeathClaim> DeathClaim { get; set; }
        public virtual DbSet<RefundContribution> RefundContribution { get; set; }


        public virtual DbSet<UserLog> UserLogs { get; set; }

        public virtual DbSet<UserLog> Wf_WorkflowDocumentType { get; set; }
        public virtual DbSet<UserLog> Wf_WorkFlowDocumentPattern { get; set; }
        public virtual DbSet<UserLog> Wf_Documents { get; set; }
        public virtual DbSet<UserLog> Wf_DocumentUsers { get; set; }
        public virtual DbSet<UserLog> Wf_DocumentLog { get; set; }

        public virtual DbSet<App_EntityLog> App_EntityLog { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<Accounts>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.CircleId).HasColumnName("CircleID");

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.YearOf).HasColumnName("YearOF");

                entity.HasOne(d => d.Circle)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.CircleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Accounts_Circle");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.AccountsCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Accounts_User");

                entity.HasOne(d => d.DpCodeNavigation)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.DpCode)
                    .HasConstraintName("FK_Accounts_Branch");

                //entity.HasOne(d => d.MonthNavigation)
                //   .WithMany(p => p.Accounts)
                //   .HasForeignKey(d => d.MonthCode)
                //   .HasConstraintName("FK_Accounts_MonthCode");

                

                entity.HasOne(d => d.ModifiedByUser)
                    .WithMany(p => p.AccountsModifiedByUser)
                    .HasForeignKey(d => d.ModifiedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Accounts_User1");

                entity.HasOne(d => d.StaffNoNavigation)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.StaffNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Accounts_Member");

            


                entity.HasOne(d => d.TransModeNavigation)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.TransMode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Accounts_Status");
            });

                   modelBuilder.Entity<AccountsDirectEntry>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.DdIba)
                    .HasColumnName("DD_IBA ")
                    .HasMaxLength(255);

                entity.Property(e => e.DdIbaDate)
                    .HasColumnName("DD_IBA Date")
                    .HasMaxLength(255);

                entity.Property(e => e.Enrl).HasMaxLength(255);

                entity.Property(e => e.F10).HasMaxLength(255);

                entity.Property(e => e.F11).HasMaxLength(255);

                entity.Property(e => e.F9).HasMaxLength(255);

                entity.Property(e => e.Fine).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);
            });

           

         
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.HasKey(e => e.DpCode);

                entity.Property(e => e.DpCode).ValueGeneratedNever();

                entity.Property(e => e.Address1)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Address3)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CircleId).HasColumnName("CircleID");

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.District)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.HasOne(d => d.Circle)
                    .WithMany(p => p.Branch)
                    .HasForeignKey(d => d.CircleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Branch_Circle");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.BranchCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Branch_User");

                entity.HasOne(d => d.ModifiedByUser)
                    .WithMany(p => p.BranchModifiedByUser)
                    .HasForeignKey(d => d.ModifiedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Branch_User1");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Branch)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("FK_Branch_State");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Abbreviation)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.CategoryCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Category_User");

                entity.HasOne(d => d.ModifiedByUser)
                    .WithMany(p => p.CategoryModifiedByUser)
                    .HasForeignKey(d => d.ModifiedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Category_User1");
            });

      
            modelBuilder.Entity<Circle>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Abbreviation)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DateFrom).HasColumnType("datetime");

                entity.Property(e => e.DateTo).HasColumnType("datetime");

                entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.CircleCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Circle_User");

                entity.HasOne(d => d.ModifiedByUser)
                    .WithMany(p => p.CircleModifiedByUser)
                    .HasForeignKey(d => d.ModifiedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Circle_User1");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Circle)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("FK_Circle_State");
            });

            modelBuilder.Entity<CircleState>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CircleId).HasColumnName("CircleID");

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.HasOne(d => d.Circle)
                    .WithMany()
                    .HasForeignKey(d => d.CircleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CircleState_Circle");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CircleState_User");

                entity.HasOne(d => d.ModifiedByUser)
                    .WithMany()
                    .HasForeignKey(d => d.ModifiedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CircleState_User1");

                entity.HasOne(d => d.State)
                    .WithMany()
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CircleState_State");
            });

            modelBuilder.Entity<Designation>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.DesignationCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Designation_User");

                entity.HasOne(d => d.ModifiedByUser)
                    .WithMany(p => p.DesignationModifiedByUser)
                    .HasForeignKey(d => d.ModifiedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Designation_User1");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Image1)
                    .IsRequired()
                    .HasColumnName("Image");

                entity.Property(e => e.ModifiedByuserId).HasColumnName("ModifiedByuserID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.ImageCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Image_User");

                entity.HasOne(d => d.ModifiedByuser)
                    .WithMany(p => p.ImageModifiedByuser)
                    .HasForeignKey(d => d.ModifiedByuserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Image_User1");
            });
            modelBuilder.Entity<MemberDefault>(eb =>
{
    eb.HasNoKey();
    eb.ToView("MemberDefaults");

});


            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.StaffNo);

                entity.Property(e => e.StaffNo).ValueGeneratedNever();

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DesignationId).HasColumnName("DesignationID");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("datetime");

                entity.Property(e => e.Doj)
                    .HasColumnName("DOJ")
                    .HasColumnType("datetime");

                entity.Property(e => e.DojtoScheme)
                    .HasColumnName("DOJToScheme")
                    .HasColumnType("datetime");

                entity.Property(e => e.GenderId).HasColumnName("GenderID");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Member)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Member_Category");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.MemberCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Member_User");

                entity.HasOne(d => d.Designation)
                    .WithMany(p => p.Member)
                    .HasForeignKey(d => d.DesignationId)
                    .HasConstraintName("FK_Member_Designation");

                entity.HasOne(d => d.DpCodeNavigation)
                    .WithMany(p => p.Member)
                    .HasForeignKey(d => d.DpCode)
                    .HasConstraintName("FK_Member_Branch");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Member)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK_Member_Image");

                entity.HasOne(d => d.ModifiedByUser)
                    .WithMany(p => p.MemberModifiedByUser)
                    .HasForeignKey(d => d.ModifiedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Member_User1");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Member)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Member_Status");
            });

      
            modelBuilder.Entity<Month>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Abbrivation)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.MonthName)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);


           

            });

               modelBuilder.Entity<State>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Abbreviation)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Abbreviation)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.StatusCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Status_User");

                entity.HasOne(d => d.ModifiedByUser)
                    .WithMany(p => p.StatusModifiedByUser)
                    .HasForeignKey(d => d.ModifiedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Status_User1");
            });

            modelBuilder.Entity<Transfer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.TransDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.TransferCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transfer_User");

                entity.HasOne(d => d.DpCodeNavigation)
                    .WithMany(p => p.Transfer)
                    .HasForeignKey(d => d.DpCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transfer_Branch");

                entity.HasOne(d => d.ModifiedByUser)
                    .WithMany(p => p.TransferModifiedByUser)
                    .HasForeignKey(d => d.ModifiedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transfer_User1");

                entity.HasOne(d => d.StaffNoNavigation)
                    .WithMany(p => p.Transfer)
                    .HasForeignKey(d => d.StaffNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transfer_Member");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.Property(e => e.ValidTill).HasColumnType("datetime");


            });

         
            modelBuilder.Entity<UserType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Abbreviation)
                    .IsRequired()
                    .HasColumnName("Abbreviation")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Member>().Property(u => u.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            OnModelCreatingPartial(modelBuilder);
        }
        public override int SaveChanges()
        {
            try
            {
                var entries = ChangeTracker
                        .Entries()
                        .Where(e =>
                                e.State == EntityState.Added
                                || e.State == EntityState.Modified);

                foreach (var entityEntry in entries)
                {
                    entityEntry.Property("ModifiedDate").CurrentValue = DateTime.Now;

                    if (entityEntry.State == EntityState.Added)
                    {
                        entityEntry.Property("CreatedDate").CurrentValue = DateTime.Now;
                    }
                }
            }
            catch (Exception)
            {

              
            }

            return base.SaveChanges();
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
