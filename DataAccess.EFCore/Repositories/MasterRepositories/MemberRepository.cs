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
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        public MemberRepository(ApplicationContext context) : base(context)
        {
        }


        public List<dynamic> GetSelectList(Expression<Func<Member, bool>> expression, int skip = 0, int nop = 0)
        {
            var q = _context.Member.AsNoTracking().Where(expression).Skip(skip).Take(nop).Select(u => new { code = u.StaffNo.ToString(), label = u.Name.ToString() + " (" + u.StaffNo.ToString() + ")" }).ToList<dynamic>();
            return q;
        }


        public Member GetemployeeProfile(int id)
        {
            var q = (from staff in _context.Member
                     join desg in _context.Designation on staff.DesignationId equals desg.Id
                     join catg in _context.Category on staff.CategoryId equals catg.Id
                     join stats in _context.Status on staff.StatusId equals stats.Id
                     where staff.StaffNo == id
                     select new Member
                     {

                         Id = staff.Id,
                         StaffNo = staff.StaffNo,
                         Name = staff.Name,
                         GenderId = staff.GenderId,
                         ImageId = staff.ImageId,
                         Dob = staff.Dob,
                         DesignationId = staff.DesignationId,
                         CategoryId = staff.CategoryId,
                         Doj = staff.Doj,
                         DpCode = staff.DpCode,
                         DojtoScheme = staff.DojtoScheme,
                         StatusId = staff.StatusId,
                         IsRegCompleted = staff.IsRegCompleted,
                         CreatedByUserId = staff.CreatedByUserId,
                         CreatedDate = staff.CreatedDate,
                         ModifiedByUserId = staff.ModifiedByUserId,
                         ModifiedDate = staff.ModifiedDate,
                         Nominee = staff.Nominee,
                         NomineeRelation = staff.NomineeRelation,
                         NomineeIDentity = staff.NomineeIDentity,
                         UnionMember = staff.UnionMember,
                         Status_text = stats.Name,
                         Gender_text = staff.GenderId == 0 ? "Male" : "Female",
                         Designation_text = desg.Description,
                         Category_text = catg.Name


                     }).FirstOrDefault();

            return q;
        }

        public List<Member> GetAllemployeeProfile()
        {

            var q = (from staff in _context.Member
                     join desg in _context.Designation on staff.DesignationId equals desg.Id
                     join catg in _context.Category on staff.CategoryId equals catg.Id
                     join stats in _context.Status on staff.StatusId equals stats.Id
                     select new Member
                     {

                         Id = staff.Id,
                         StaffNo = staff.StaffNo,
                         Name = staff.Name,
                         GenderId = staff.GenderId,
                         ImageId = staff.ImageId,
                         Dob = staff.Dob,
                         DesignationId = staff.DesignationId,
                         CategoryId = staff.CategoryId,
                         Doj = staff.Doj,
                         DpCode = staff.DpCode,
                         DojtoScheme = staff.DojtoScheme,
                         StatusId = staff.StatusId,
                         IsRegCompleted = staff.IsRegCompleted,
                         CreatedByUserId = staff.CreatedByUserId,
                         CreatedDate = staff.CreatedDate,
                         ModifiedByUserId = staff.ModifiedByUserId,
                         ModifiedDate = staff.ModifiedDate,
                         Nominee = staff.Nominee,
                         NomineeRelation = staff.NomineeRelation,
                         NomineeIDentity = staff.NomineeIDentity,
                         UnionMember = staff.UnionMember,
                         Status_text = stats.Name,
                         Gender_text = staff.GenderId == 0 ? "Male" : "Female",
                         Designation_text = desg.Description,
                         Category_text = catg.Name

                     }).AsNoTracking().ToList();

            return q;
        }


        public List<string> GetAllStaff()
        {

            var q = _context.Member.Select(u => u.StaffNo.ToString()).ToList();

            return q;
        }


        public IQueryable<Member> GetAllMemberasync(Expression<Func<Member, bool>> expression)
        {
            var result = (from staff in _context.Member
                          join desg in _context.Designation on staff.DesignationId equals desg.Id
                          join catg in _context.Category on staff.CategoryId equals catg.Id
                          join stats in _context.Status on staff.StatusId equals stats.Id
                          select new Member
                          {

                              Id = staff.Id,
                              StaffNo = staff.StaffNo,
                              Name = staff.Name,
                              GenderId = staff.GenderId,
                              ImageId = staff.ImageId,
                              Dob = staff.Dob,
                              DesignationId = staff.DesignationId,
                              CategoryId = staff.CategoryId,
                              Doj = staff.Doj,
                              DpCode = staff.DpCode,
                              DojtoScheme = staff.DojtoScheme,
                              StatusId = staff.StatusId,
                              IsRegCompleted = staff.IsRegCompleted,
                              CreatedByUserId = staff.CreatedByUserId,
                              CreatedDate = staff.CreatedDate,
                              ModifiedByUserId = staff.ModifiedByUserId,
                              ModifiedDate = staff.ModifiedDate,
                              Nominee = staff.Nominee,
                              NomineeRelation = staff.NomineeRelation,
                              NomineeIDentity = staff.NomineeIDentity,
                              UnionMember = staff.UnionMember,
                              Status_text = stats.Name,
                              Gender_text = staff.GenderId == 0 ? "Male" : "Female",
                              Designation_text = desg.Description,
                              Category_text = catg.Name

                          }).AsNoTracking().AsQueryable();
            result = result.Where(expression);

            return result;
        }

    }

}
