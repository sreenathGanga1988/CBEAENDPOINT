using Domain.Entities;
using Domain.Interfaces;

namespace DataAccess.EFCore.Repositories
{
    public class SupportTicketRepository : GenericRepository<SupportTicket>, ISupportTicketRepository
    {
        public SupportTicketRepository(ApplicationContext context) : base(context)
        {
        }
    }

}
