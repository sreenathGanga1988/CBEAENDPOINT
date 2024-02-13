using Dapper;
using Domain.Entities.BaseEntities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DataAccess.EFCore.Repositories
{
    public class ReportRepository : IReportRepository
    {
        protected readonly ApplicationContext _context;

        private readonly IDbConnection _dbConnection;

        public ReportRepository(ApplicationContext context, IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }


        public List<dynamic> GetReport(ReportParameterlist reportParameterlist)
        {
            try
            {
                if (reportParameterlist.ReportType == "DEFAULTER")
                {

                    //var q = _context.MemberDefaults.Where(u => u.PendingContribution > 6).ToList<dynamic>();
                    //return q;


                    const string sql = @"SELECT * FROM MemberDefaults";
                    dynamic account = _dbConnection.Query<IEnumerable<dynamic>>(sql);
                 return account;

                    

                }
                else
                {
                    return new List<dynamic>();
                }
            }
            catch (Exception ex)
            {

                throw;

            }

        }

        public DataTable GetDataReport(String ReportType, string id)
        {
            DataTable dataTable = new DataTable();
            try
            {
                string sql = "";
                if (ReportType.Trim().ToUpper() == "NewMembers".Trim().ToUpper())
                {
                    sql = "select  Id, ContributionMasterId as CMID, Circle, Month, Year, DpCode, StaffNo, Name, Designation,   Amount, isParked,FullString as [Bank Record] from contributiondetails where ContributionMasterId= " + id + " and StaffNo  not in (select distinct StaffNo from Member)";
                }
                else if (ReportType.Trim().ToUpper() == "WrongBranch".Trim().ToUpper())
                {
                    sql = "select  Id, ContributionMasterId as CMID, Circle, Month, Year, DpCode, StaffNo, Name, Designation,   Amount, isParked ,FullString as [Bank Record] from contributiondetails where ContributionMasterId= " + id + "  and dpcode  not in (select distinct dpcode from branch)";
                }
                else if (ReportType.Trim().ToUpper() == "WrongCircle".Trim().ToUpper())
                {
                    sql = "select  Id,  ContributionMasterId as CMID,  Circle, Month, Year, DpCode, StaffNo, Name, Designation, Amount, isParked,FullString as [Bank Record] from contributiondetails where ContributionMasterId= " + id + " and circle  not in (select distinct circlecode from Circle)";
                }
                else if (ReportType.Trim().ToUpper() == "ParkedItem".Trim().ToUpper())
                {
                    sql = "select  Id, ContributionMasterId as CMID, Circle, Month, Year, DpCode, StaffNo, Name, Designation,   Amount,ParkReason as [Parked reason],FullString as [Bank Record] from contributiondetails where ContributionMasterId= " + id + " and isParked=1";
                }
                else if (ReportType.Trim().ToUpper() == "All".Trim().ToUpper())
                {
                    sql = "select  Id,ContributionMasterId as CMID,  Circle, Month, Year, DpCode, StaffNo, Name, Designation,   Amount, isParked,FullString as [Bank Record] from contributiondetails where ContributionMasterId= " + id +"";
                }
                else if (ReportType.Trim().ToUpper() == "Defaulter".Trim().ToUpper())
                {
                    sql = "exec GetMemberDefaults";
                }

                dataTable = _context.DataTable(sql);
            }
            catch (Exception ex)
            {

                throw;

            }

            return dataTable;
        }

        public DataTable GetDataReport(String sqlstring)
        {
            DataTable dataTable = new DataTable();
            try
            {
               

                dataTable = _context.DataTable(sqlstring);
            }
            catch (Exception ex)
            {

                throw;

            }

            return dataTable;
        }
       
    }


}
