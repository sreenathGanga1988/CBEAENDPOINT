using Domain.Entities;
using Domain.Interfaces;

namespace DataAccess.EFCore.Repositories
{
    public class ManagingComiteeRepository : GenericRepository<ManagingComitee>, IManagingComiteeRepository
    {
        public ManagingComiteeRepository(ApplicationContext context) : base(context)
        {
        }
    }

}
