using Domain.Entities;
using Domain.Interfaces;

namespace DataAccess.EFCore.Repositories
{
    public class CircleStateRepository : GenericRepository<CircleState>, ICircleStateRepository
    {
        public CircleStateRepository(ApplicationContext context) : base(context)
        {
        }
    }

}
