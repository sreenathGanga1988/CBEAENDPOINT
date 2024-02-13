using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        bool DataExists(Expression<Func<T, bool>> expression);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> filter = null,
                                          Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
                                          string includeProperties = "", int first = 0, int offset = 0);
        int DataCount(Expression<Func<T, bool>> expression);




        Task<T> AddAsyn(T t);
        Task<int> DataCountAsync();
        Task<int> DeleteAsyn(T entity);
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);
        Task<T> FindItemAsync(Expression<Func<T, bool>> match);
        Task<ICollection<T>> FindByAsyn(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> GetAllAsyn();
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);

        DataSet GetFilteredPaginatedData(String ReportType, String SearchText="", int PageSize = 0, int PageNumber = 0);
    }
}