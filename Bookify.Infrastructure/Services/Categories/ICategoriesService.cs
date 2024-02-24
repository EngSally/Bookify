


using System.Linq.Expressions;

namespace Bookify.Infrastructure.Services
{
    public  interface ICategoriesService
    {
        IEnumerable<Category> GetAll(bool asNoTracking = false);
        Category Add(Category author);
        Category? GetById(int id);
        void Update(Category author);
        Category? Find(Expression<Func<Category, bool>> predicate);
    }
}
