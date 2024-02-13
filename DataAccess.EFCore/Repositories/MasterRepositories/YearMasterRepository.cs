using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DataAccess.EFCore.Repositories
{
    public class YearMasterRepository : GenericRepository<YearMaster>, IYearMasterRepository
    {
        public YearMasterRepository(ApplicationContext context) : base(context)
        {
        }

        public List<dynamic> GetSelectList(Expression<Func<YearMaster, bool>> expression, int skip = 0, int nop = 0)
        {


            var k = _context.YearMasters.AsNoTracking().Where(expression).AsQueryable();

            if (skip != 0)
            {
                k = k.Skip(skip);
            }
            if (nop != 0)
            {
                k = k.Take(nop);
            }
            var q = k.Select(u => new { code = u.YearName.ToString(), label = u.YearName.ToString() }).ToList<dynamic>();
            return q;


        }
    }

}
