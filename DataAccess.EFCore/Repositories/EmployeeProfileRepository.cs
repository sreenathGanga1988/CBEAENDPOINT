using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EFCore.Repositories
{
   public class EmployeeProfileRepository : IEmployeeProfileRepository
    {
        protected readonly ApplicationContext _context;
        public EmployeeProfileRepository(ApplicationContext context) 
        {
            _context = context;
        }

        
        public  List<dynamic>  GetemployeeProfile( int id )
        {
            var q= (from staff in _context.Member
                   join desg in  _context.Designation on staff.DesignationId equals desg.Id               
                   join  catg in _context.Category on staff.CategoryId equals catg.Id 
                   join stats in _context.Status  on staff.StatusId equals stats.Id
                    where staff.StaffNo==id
                    select new
                   {
                          StaffNo=staff.StaffNo,
                          Name=staff.Name ,
                          Gender="Male",
                          Designation=desg.Description ,
                          Category=catg.Name,
                          DateofBirth=staff.Dob,
                          DateofJoin=staff.Doj,
                          DpCode=staff.DpCode,
                          DateFrom=staff.DojtoScheme,
                          DateTo=DateTime.Now,
                          RetirementDate=DateTime.Now,
                          Status=stats.Name,
                          Nominee=staff.Nominee ?? "",
                          NomineeRelation=staff.NomineeRelation ?? "",

                    }).ToList<dynamic>();

            return q;
        }

    }
}
