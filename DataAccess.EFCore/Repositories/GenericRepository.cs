using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EFCore.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationContext _context;


        public GenericRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }


        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public bool DataExists(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Any(expression);

        }
        public int DataCount(Expression<Func<T, bool>> expression)
        {
            int count = 0;
            try
            {
                count = _context.Set<T>().Where(expression).Count();
            }
            catch (Exception)
            {


            }

            return count;

        }
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> filter = null,
                                                Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
                                                string includeProperties = "", int first = 0, int offset = 0)
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (offset > 0)
            {
                query = query.Skip(offset);
            }
            if (first > 0)
            {
                query = query.Take(first);
            }
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderby != null)
            {
                return await orderby(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }






        public virtual async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public virtual async Task<ICollection<T>> GetAllAsyn()
        {

            return await _context.Set<T>().ToListAsync();
        }
        public virtual async Task<T> AddAsyn(T t)
        {
            _context.Set<T>().Add(t);
            await _context.SaveChangesAsync();
            return t;

        }
        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(match);
        }

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().Where(match).ToListAsync();
        }
        public virtual async Task<int> DeleteAsyn(T entity)
        {
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync();
        }
        public virtual async Task<T> UpdateAsyn(T t, object key)
        {
            if (t == null)
                return null;
            T exist = await _context.Set<T>().FindAsync(key);
            if (exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(t);
                await _context.SaveChangesAsync();
            }
            return exist;
        }
        public async Task<int> DataCountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }
        public virtual async Task<T> FindItemAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(match);
        }


        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> queryable = _context.Set<T>();
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {

                queryable = queryable.Include<T, object>(includeProperty);
            }

            return queryable;
        }

        public async virtual Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public virtual async Task<ICollection<T>> FindByAsyn(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public DataSet GetFilteredPaginatedData(String ReportType, String SearchText = "", int PageSize = 0, int PageNumber = 0)
        {
            DataSet dset = new DataSet();
            try
            {

                String qry = "exec SP_GetFilteredPaginatedData @ReportType='" + ReportType + "', @SearchText='" + SearchText + "'," +
                    "@Pagenumber=" + PageNumber + ",@pageSize=" + PageNumber + "";

                dset = _context.DataSet(qry);
            }
            catch (Exception ex)
            {

                throw;

            }

            return dset;
        }

    }
}
