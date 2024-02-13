using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Dapper;

namespace DataAccess.EFCore.Repositories
{
    public class ContributionMasterRepository : GenericRepository<ContributionMaster>, IContributionMasterRepository
    {
        private readonly IDbConnection _dbConnection;

        public ContributionMasterRepository(ApplicationContext context, IDbConnection dbConnection) : base(context)
        {

            _dbConnection = dbConnection;
        }

        public List<dynamic> GetContribution(int contributionID, int isapproved)
        {


            string sql = @"SELECT * FROM VW_ContributionMasterStatus where isApproved =" + isapproved.ToString();
            if (contributionID != 0)
            {
                sql = sql + " id = " + contributionID.ToString();
            }

            try
            {
                dynamic account = _dbConnection.Query<dynamic>(sql);
                return account;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public Boolean Approve(int ID, int UseriD, Boolean approve)
        {
            Boolean issucess = false;



            try
            {
                var contrmaster = _context.ContributionMasters.Where(u => u.Id == ID).FirstOrDefault();
                if (contrmaster != null)
                {
                    if (approve == true)
                    {
                        contrmaster.isApproved = true;
                        contrmaster.ContributionStatus = "A";
                        contrmaster.ApprovedDate = DateTime.UtcNow.ToString();
                        contrmaster.ApprovedBy = UseriD.ToString();

                        var q = (from contrdet in _context.ContributionDetails
                                 join circ in _context.Circle on contrdet.Circle equals circ.CircleCode
                                 where contrdet.ContributionMasterId == contrmaster.Id & contrdet.isParked == false
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
                        issucess = true;
                    }
                    else
                    {
                        contrmaster.isApproved = false;
                        contrmaster.ContributionStatus = "R";
                        contrmaster.ApprovedDate = DateTime.UtcNow.ToString();
                        contrmaster.ApprovedBy = UseriD.ToString();
                        issucess = true;
                    }



                }
            }
            catch (Exception ex)
            {

                throw;
            }





            return issucess;
        }



        public List<dynamic> GetAllContribution()
        {
            string sql = @"SELECT * FROM VW_ContributionMasterStatus order by  year desc, monthcode desc";


            try
            {
                dynamic account = _dbConnection.Query<dynamic>(sql);
                return account;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public DataTable GetTotalContribution()
        {
            DataTable dttemp = new DataTable();
            var dt = new DataTable();

            dt = _context.DataTable("SELECT SUM(Amount) AS TotalAmount, YearOF, MonthCode, COUNT(StaffNo) AS CountofMember FROM  Accounts GROUP BY YearOF, MonthCode");

            //using (var da = new SqlDataAdapter(, @"Data Source=(LocalDb)\MyInstance;Initial Catalog=DB_A424F9_gjcbeu;Integrated Security=True"))
            //{
            //    da.Fill(dt);
            //}

            DataView dvyear = new DataView(dt);
            dvyear.Sort = "yearOf desc";


            dttemp.Columns.Add("Year");
            for (int i = 1; i < 13; i++)
            {

                dttemp.Columns.Add("Member_" + i.ToString());
                dttemp.Columns.Add("Amount_" + i.ToString());
            }

            dttemp.Columns.Add("TotalYearAmount");


            DataTable distinctyears = dvyear.ToTable(true, "YearOF");



            foreach (DataRow item in distinctyears.Rows)
            {
                DataRow dtow = dttemp.NewRow();
                dtow["Year"] = item["YearOF"];

                double TotalYearAmount = 0;
                for (int i = 1; i < 13; i++)
                {
                    Tuple<string, String> tuple = GetTotalAmount(int.Parse(item["YearOF"].ToString()), i, ref dt);
                    dtow["Amount_" + i] = tuple.Item1;

                    dtow["Member_" + i] = tuple.Item2;

                    TotalYearAmount = TotalYearAmount + Double.Parse(tuple.Item1);
                }

                dtow["TotalYearAmount"] = Math.Round(TotalYearAmount, 2);






                dttemp.Rows.Add(dtow);
            }

            return dttemp;
        }

        private Tuple<string, string> GetTotalAmount(int year, int month, ref DataTable dt)
        {
            double _totalAmount = 0;
            double _totalMember = 0;

            DataRow[] tmpDatarows = dt.Select("YearOF=" + year + " and MonthCode =" + month + " ");

            if (tmpDatarows != null)
            {
                foreach (var itemproj in tmpDatarows)
                {
                    _totalAmount = _totalAmount + double.Parse(itemproj["TotalAmount"].ToString());
                    _totalMember = _totalMember + double.Parse(itemproj["CountofMember"].ToString());

                }


            }
            return Tuple.Create(_totalAmount.ToString(), _totalMember.ToString());
        }



        public DataTable GetMemberDefaults()
        {
          
            var dt = new DataTable();

            dt = _context.DataTable("exec GetMemberDefaults");

          

            return dt;
        }



        
    }

}
