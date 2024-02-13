using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Dapper;

namespace DataAccess.EFCore.Repositories
{
    public class ContributionDetailRepository : GenericRepository<ContributionDetail>, IContributionDetailRepository
    {
        private readonly IDbConnection _dbConnection;

        public ContributionDetailRepository(ApplicationContext context, IDbConnection dbConnection) : base(context)
        {

            _dbConnection = dbConnection;
        }


        public int GetWrongCircle(int contributionID)
        {
            const string sql = @"SELECT * FROM MemberDefaults";
            int wrong = _dbConnection.Query<int>(sql).FirstOrDefault();

            return wrong;
        }

        public int GetWrongDpCode(int contributionID)
        {
            const string sql = @"SELECT * FROM MemberDefaults";
            int wrong = _dbConnection.Query<int>(sql).FirstOrDefault();

            return wrong;
        }

        public int GetWrongMembers(int contributionID)
        {
            const string sql = @"Select Count (*) from ContributionDetails  where  StaffNo not in (select distinct StaffNo from Member)";
            int wrong = _dbConnection.Query<int>(sql).FirstOrDefault();

            return wrong;
        }




        public List<Accounts> GetAccounts(int contributionID)
        {

            var q = (from contrdet in _context.ContributionDetails
                     where
                     contrdet.ContributionMasterId == contributionID
                     select new Accounts
                     {

                         CircleId = int.Parse(contrdet.Circle.ToString()),
                         DpCode = int.Parse(contrdet.DpCode.ToString()),
                         StaffNo = int.Parse(contrdet.StaffNo.ToString()),
                         Amount = decimal.Parse(contrdet.Amount.ToString()),
                         MonthCode = int.Parse(contrdet.Month.ToString()),
                         YearOf = int.Parse(contrdet.Year.ToString()),
                     }).ToList();

            return q;

        }

        public Boolean UnParkContribution(int ContributiondetailId, int UseriD, out string Message)
        {
            Boolean issucess = false;

            try
            {
                var item = _context.ContributionDetails.Where(u => u.Id == ContributiondetailId).FirstOrDefault();



                if (item.isParked == true)
                {

                    int staffno = int.Parse(item.StaffNo);
                    int year = int.Parse(item.Year);
                    int month = int.Parse(item.Month);

                    var accont = _context.Accounts.Where(u => u.StaffNo == staffno & u.YearOf == year & u.MonthCode == month).FirstOrDefault();

                    if (accont == null)
                    {
                        var q = (from contrdet in _context.ContributionDetails
                                 join circ in _context.Circle on contrdet.Circle equals circ.CircleCode
                                 where contrdet.Id == ContributiondetailId
                                 select new Accounts
                                 {
                                     CircleId = int.Parse(circ.Id.ToString()),
                                     DpCode = int.Parse(contrdet.DpCode.ToString()),
                                     StaffNo = int.Parse(contrdet.StaffNo.ToString()),
                                     MonthCode = int.Parse(contrdet.Month.ToString()),
                                     YearOf = int.Parse(contrdet.Year.ToString()),
                                     Amount = decimal.Parse(contrdet.Amount.ToString()),
                                     Reference = "Bank File",
                                     Remark = contrdet.ContributionMasterId.ToString() + "/" + contrdet.Id,
                                     CreatedByUserId = UseriD,
                                     ModifiedByUserId = UseriD,
                                     TransMode = 8,
                                 }).ToList();
                        _context.Accounts.AddRange(q);


                        var contrdetail = _context.ContributionDetails.Where(u => u.Id == ContributiondetailId).FirstOrDefault();
                        contrdetail.isParked = false;
                        contrdetail.UnParkedon = DateTime.Now;
                        
                        _context.SaveChanges();
                        issucess = true;
                        Message = "Unparked Sucessfully";
                    }
                    else
                    {
                        Message = "Contribution Already Added";
                    }

                }
                else
                {
                    Message = "Item Already Unparked";
                }

            }
            catch (Exception ex)
            {
              var k=  ex.Message;
                throw;
            }
            return issucess;
        }


    }

}
