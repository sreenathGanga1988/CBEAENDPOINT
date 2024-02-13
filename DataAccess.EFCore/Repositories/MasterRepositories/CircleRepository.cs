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
    public class CircleRepository : GenericRepository<Circle>, ICircleRepository
    {
        public CircleRepository(ApplicationContext context) : base(context)
        {
        }


        public List<dynamic> GetSelectList(Expression<Func<Circle, bool>> expression, int skip = 0, int nop = 0)
        {
            var k = _context.Circle.AsNoTracking().Where(expression).AsQueryable();
            k = k.Where(u => u.IsActive == true).AsQueryable();
            if (skip != 0)
            {
                k = k.Skip(skip);
            }
            if (nop != 0)
            {
                k = k.Take(nop);
            }
            var q = k.Select(u => new { code = u.Id.ToString(), label = (u.CircleCode ?? 0).ToString() + " / " + u.Name.ToString() }).ToList<dynamic>();
            //var q = k.Select(u => new { code = u.Id.ToString(), label = (u.CircleCode ?? 0).ToString() + " / " + u.Name.ToString() + " (" + u.CircleCode.ToString() + ")" }).ToList<dynamic>();
            return q;
        }
    }

}
