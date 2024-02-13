using Domain.Entities;
using Domain.Interfaces;

namespace DataAccess.EFCore.Repositories
{
    public class TransferRepository : GenericRepository<Transfer>, ITransferRepository
    {
        public TransferRepository(ApplicationContext context) : base(context)
        {
        }
    }

}
