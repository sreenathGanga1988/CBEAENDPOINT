using Domain.Interfaces;
using Domain;

namespace DataAccess.EFCore.Repositories
{
    public class NewsItemRepository : GenericRepository<NewsItem>, INewsItemRepository
    {
        public NewsItemRepository(ApplicationContext context) : base(context)
        {
        }
    }

}
