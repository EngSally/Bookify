using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Common.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        IQueryable<T> GetQueryable();
        IEnumerable<T> GetAll(bool withTracking=true );
       T Add(T entity);
       T? GetById(int id);
       T? Find(Expression<Func<T, bool>> predicate);
       T? Find(Expression<Func<T, bool>> predicate,
               Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        IEnumerable<T> AddRange(IEnumerable<T> entities);
        void Update(T entity);
        int Count();
        public int Count(Expression<Func<T, bool>> predicate);
        public int Max(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> field);


    }
}
