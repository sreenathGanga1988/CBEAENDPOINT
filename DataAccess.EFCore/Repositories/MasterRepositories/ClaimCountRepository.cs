using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EFCore.Repositories
{
    public class ClaimCountRepository : GenericRepository<ClaimCount>, IClaimCountRepository
    {
        public ClaimCountRepository(ApplicationContext context) : base(context)
        {
        }

        public List<ClaimCount> GetclaimCount()
        {
            var query = @"SELECT        State.Name AS StateName, Designation.Description AS Designation, ClaimCounts.YearOF AS YearOF, SUM(ClaimCounts.Count) AS Claim, ClaimCounts.StateId, ClaimCounts.DesignationId
 FROM            ClaimCounts INNER JOIN
                          Designation ON ClaimCounts.Id = Designation.ID INNER JOIN
                         State ON ClaimCounts.StateId = State.ID
GROUP BY State.Name, Designation.Description, ClaimCounts.YearOF, ClaimCounts.StateId, ClaimCounts.DesignationId";

            var books = _context.ClaimCounts.FromSqlRaw(query).ToList();


            return books;
        }

    }

}
