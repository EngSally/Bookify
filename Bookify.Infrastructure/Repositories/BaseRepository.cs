


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Bookify.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
                _context = context;
        }
        public IEnumerable<T> GetAll( bool withTracking = true )
        {
            IQueryable<T> quary = _context.Set<T>();
            if (!withTracking)
                return quary.AsNoTracking();
            return quary.ToList(); 
         
        }

        public T Add(T entity)
        {
           _context.Set<T>().Add(entity);
           //_context.SaveChanges();
            return entity;
        }

        public T? GetById(int id)=> _context.Set<T>().Find( id);  
        

        public T? Find(Expression<Func<T, bool>> predicate)=>_context.Set<T>().FirstOrDefault(predicate);

        public T? Find(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> quary = _context.Set<T>().AsQueryable();
            if (include != null)
                quary=include(quary);
            return quary.SingleOrDefault(predicate);


        }
        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            _context.AddRange(entities);
            return entities;
        }
        public void Update(T entity) => _context.Update(entity);
        public int Count() => _context.Set<T>().Count();

        public int Count(Expression<Func<T, bool>> predicate) => _context.Set<T>().Count(predicate);

        public int Max(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> field) =>
            _context.Set<T>().Any(predicate) ? _context.Set<T>().Where(predicate).Max(field) : 0;
    }

}
