using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.EFCore.Repositories
{
    public class DeathClaimRepository : GenericRepository<DeathClaim>, IDeathClaimRepository
    {
        public DeathClaimRepository(ApplicationContext context) : base(context)
        {
        }

        public List<dynamic> GetAllClaims()
        {
            var a = (from Dtclaims in _context.DeathClaim
                    join mymember in _context.Member on Dtclaims.StaffNo equals mymember.StaffNo
                    join desg in _context.Designation on Dtclaims.DesignationId equals desg.Id 

                     select new {id=Dtclaims.Id,StaffNo=Dtclaims.StaffNo ,
                         StaffName = mymember.Name 
                     ,BranchName=desg.Name ,
                         DDDATE = Dtclaims.DDDATE,Amount=Dtclaims.Amount }).ToList<dynamic>();

            return a;

        }

     

    }  

}
