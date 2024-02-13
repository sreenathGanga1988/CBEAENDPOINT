using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Linq;
using System.Data;

namespace DataAccess.EFCore.Repositories
{
    public class AccountsDirectEntryRepository : GenericRepository<AccountsDirectEntry>, IAccountsDirectEntryRepository
    {
        public AccountsDirectEntryRepository(ApplicationContext context) : base(context)
        {
        }
        public Boolean Approve(int ID, int UseriD, Boolean approve)
        {
            Boolean issucess = false;
            var directEntry = _context.AccountsDirectEntry.Find(ID);
            if (directEntry != null)
            {

                var circleid = _context.Branch.Where(u => u.DpCode == directEntry.DpCode).Select(u => u.CircleId).FirstOrDefault();

                if (approve == true)
                {
                    directEntry.isApproved = true;
                    directEntry.status = "A";
                    directEntry.ApprovedDate = DateTime.UtcNow.ToString();
                    directEntry.ApprovedBy = UseriD.ToString();
                    issucess = true;


                    Accounts accounts = new Accounts();
                    accounts.CircleId = circleid;
                    accounts.DpCode = int.Parse(directEntry.DpCode.ToString());
                    accounts.StaffNo = int.Parse(directEntry.StaffNo.ToString());
                    accounts.MonthCode = int.Parse(directEntry.MonthCode.ToString());
                    accounts.YearOf = int.Parse(directEntry.YearOf.ToString());
                    accounts.Amount = decimal.Parse(directEntry.Amt.ToString());
                    accounts.Reference = "Direct Entry";
                    accounts.Remark = "Approval";
                    accounts.TransMode = 8;
                    accounts.CreatedByUserId = UseriD;
                    accounts.ModifiedByUserId = UseriD;

                }
                else
                {
                    directEntry.isApproved = false;
                    directEntry.status = "R";
                    directEntry.ApprovedDate = DateTime.UtcNow.ToString();
                    directEntry.ApprovedBy = UseriD.ToString();
                    issucess = true;
                }
            }
            return issucess;
        }
    }

}
