using Domain.Entities;
using Domain.Interfaces;

namespace DataAccess.EFCore.Repositories
{
    public class DayQuoteRepository : GenericRepository<DayQuote>, IDayQuoteRepository
    {
        public DayQuoteRepository(ApplicationContext context) : base(context)
        {
        }
    }

}
