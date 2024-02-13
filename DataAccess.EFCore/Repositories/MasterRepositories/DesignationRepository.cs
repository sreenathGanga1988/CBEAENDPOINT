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
    public class DesignationRepository : GenericRepository<Designation>, IDesignationRepository
    {
        public DesignationRepository(ApplicationContext context) : base(context)
        {
        }

        public List<dynamic> GetSelectList(Expression<Func<Designation, bool>> expression, int skip = 0, int nop = 0)
        {
            var k = _context.Designation.AsNoTracking().Where(expression).AsQueryable();

            if (skip != 0)
            {
                k = k.Skip(skip);
            }
            if (nop != 0)
            {
                k = k.Take(nop);
            }
            //   var q = k.Select(u => new { code = u.Id.ToString(), label = u.Name.ToString() }).ToList<dynamic>();
            var q = k.Select(u => new { code = u.Id.ToString(), label = u.Id.ToString() + " /" + u.Name.ToString() }).ToList<dynamic>();
            return q;
        }
    }

}
