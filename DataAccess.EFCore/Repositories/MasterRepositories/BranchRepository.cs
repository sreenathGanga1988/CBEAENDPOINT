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
    public class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {
        public BranchRepository(ApplicationContext context) : base(context)
        {
        }


        public IQueryable<Branch> GetAllBranchasync(Expression<Func<Branch, bool>> expression)
        {
            var result = (from brnch in _context.Branch
                          join state in _context.State on brnch.StateId equals state.Id
                          join circ in _context.Circle on brnch.CircleId equals circ.Id
                          select new Branch
                          {

                              Id = brnch.Id,
                              DpCode = brnch.DpCode,
                              CircleId = brnch.CircleId,
                              StateId = brnch.StateId,
                              Name = brnch.Name,
                              Address1 = brnch.Address1,
                              Address2 = brnch.Address2,
                              Address3 = brnch.Address3,
                              District = brnch.District,
                              IsActive = brnch.IsActive,
                              IsRegCompleted = brnch.IsRegCompleted,
                              CreatedByUserId = brnch.CreatedByUserId,
                              CreatedDate = brnch.CreatedDate,
                              ModifiedByUserId = brnch.CreatedByUserId,
                              ModifiedDate = brnch.ModifiedDate,
                              Circle_text = circ.Name,
                              State_text = state.Name,
                              Status = brnch.Status
                          }).AsNoTracking().AsQueryable();

            result = result.Where(expression);


            return result;
        }


        public Branch GetBrachDetail(int id)
        {

            var q = (from brnch in _context.Branch
                     join state in _context.State on brnch.StateId equals state.Id
                     join circ in _context.Circle on brnch.CircleId equals circ.Id
                     where brnch.DpCode == id
                     select new Branch
                     {

                         Id = brnch.Id,
                         DpCode = brnch.DpCode,
                         CircleId = brnch.CircleId,
                         StateId = brnch.StateId,
                         Name = brnch.Name,
                         Address1 = brnch.Address1,
                         Address2 = brnch.Address2,
                         Address3 = brnch.Address3,
                         District = brnch.District,
                         IsActive = brnch.IsActive,
                         IsRegCompleted = brnch.IsRegCompleted,
                         CreatedByUserId = brnch.CreatedByUserId,
                         CreatedDate = brnch.CreatedDate,
                         ModifiedByUserId = brnch.CreatedByUserId,
                         ModifiedDate = brnch.ModifiedDate,
                         Circle_text = circ.Name,
                         State_text = state.Name,
                     }).FirstOrDefault();

            return q;
        }

        public List<Branch> GetBrachDetails()
        {
            var q = (from brnch in _context.Branch
                     join state in _context.State on brnch.StateId equals state.Id
                     join circ in _context.Circle on brnch.CircleId equals circ.Id
                     select new Branch
                     {

                         Id = brnch.Id,
                         DpCode = brnch.DpCode,
                         CircleId = brnch.CircleId,
                         StateId = brnch.StateId,
                         Name = brnch.Name,
                         Address1 = brnch.Address1,
                         Address2 = brnch.Address2,
                         Address3 = brnch.Address3,
                         District = brnch.District,
                         IsActive = brnch.IsActive,
                         IsRegCompleted = brnch.IsRegCompleted,
                         CreatedByUserId = brnch.CreatedByUserId,
                         CreatedDate = brnch.CreatedDate,
                         ModifiedByUserId = brnch.CreatedByUserId,
                         ModifiedDate = brnch.ModifiedDate,
                         Circle_text = circ.Name,
                         State_text = state.Name,
                     }).ToList();

            return q;
        }



        public List<dynamic> GetSelectList(Expression<Func<Branch, bool>> expression, int skip = 0, int nop = 0)
        {

            var k = _context.Branch.AsNoTracking().Where(expression).AsQueryable();

            if (skip != 0)
            {
                k = k.Skip(skip);
            }
            if (nop != 0)
            {
                k = k.Take(nop);
            }
            var q = k.Select(u => new { code = u.DpCode.ToString(), label = u.DpCode.ToString() + " / " + u.Name.ToString() }).ToList<dynamic>();
            return q;





        }
    }

}
