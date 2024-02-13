using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DataAccess.EFCore.Repositories
{
    public class AccountsRepository : GenericRepository<Accounts>, IAccountsRepository
    {
        public AccountsRepository(ApplicationContext context) : base(context)
        {
        }

        public List<dynamic> GetContribution(int staffno)
        {
            var q = (from brnch in _context.Accounts where brnch.StaffNo == staffno select new { Staffno = brnch.StaffNo, Year = brnch.YearOf, Month = brnch.MonthCode, Amount = brnch.Amount }).ToList<dynamic>();


            return q;
        }


        public List<dynamic> GetContribution(int Year = 0, int monthcode = 0, int DpCode = 0, int circleCode = 0, int staffno = 0)
        {
            var Contridata = _context.Accounts.AsQueryable();

            if (staffno != 0)
            {
                Contridata = Contridata.Where(u => u.StaffNo == staffno);
            }
            if (Year != 0)
            {
                Contridata = Contridata.Where(u => u.YearOf == Year);
            }
            if (monthcode != 0)
            {
                Contridata = Contridata.Where(u => u.MonthCode == monthcode);
            }
            if (DpCode != 0)
            {
                Contridata = Contridata.Where(u => u.DpCode == DpCode);
            }
            if (circleCode != 0)
            {
                Contridata = Contridata.Where(u => u.CircleId == circleCode);
            }

            var q = Contridata.Include(u => u.StaffNoNavigation).Select(u => new { Staffno = u.StaffNo, staffname = u.StaffNoNavigation.Name, Year = u.YearOf, Month = u.MonthCode, Amount = u.Amount }).ToList<dynamic>();
            //   var q = (from brnch in _context.Accounts where brnch.StaffNo == staffno select new { Staffno = brnch.StaffNo, Year = brnch.YearOf, Month = brnch.MonthCode, Amount = brnch.Amount }).ToList<dynamic>();


            return q;
        }


        //public List<dynamic> GetYearlyContribution()
        //{


        //}


    }

}
