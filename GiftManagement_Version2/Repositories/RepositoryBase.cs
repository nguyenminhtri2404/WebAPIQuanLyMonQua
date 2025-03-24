using GiftManagement_Version2.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GiftManagement_Version2.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        T Create(T entity);
        T Update(T entity);
        void Delete(T entity);
        void CreateMulti(List<T> entities);
        void DeleteMulti(List<T> entities);
        //Find by condition
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate);
        //Find all
        IQueryable<T> FindAll();
        //Include
        IQueryable<T> Include(params Expression<Func<T, object>>[] includes);

    }

    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly MyDBContext dbContext;
        public RepositoryBase(MyDBContext dbContext)
        {
            this.dbContext = dbContext;

        }

        public T Create(T entity) => dbContext.Set<T>().Add(entity).Entity;

        public void CreateMulti(List<T> entities) => dbContext.Set<T>().AddRange(entities);

        public void Delete(T entity) => dbContext.Set<T>().Remove(entity);

        public void DeleteMulti(List<T> entities) => dbContext.Set<T>().RemoveRange(entities);

        public IQueryable<T> FindAll() => dbContext.Set<T>().AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) => dbContext.Set<T>().Where(expression).AsNoTracking();

        public T Update(T entity) => dbContext.Set<T>().Update(entity).Entity;

        public IQueryable<T> Include(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbContext.Set<T>().AsQueryable();
            return includes.Aggregate(query, (current, include) => current.Include(include));
        }
    }
}




