using Domain.Entities;
using Domain.Interfaces;

namespace DataAccess.EFCore.Repositories
{
    public class MainPageRepository : GenericRepository<MainPage>, IMainPageRepository
    {
        public MainPageRepository(ApplicationContext context) : base(context)
        {
        }
    }

}
