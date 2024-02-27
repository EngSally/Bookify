

using System.Linq.Expressions;

namespace Bookify.Infrastructure.Services
{
    internal class CategoriesService : ICategoriesService
    { 
        private readonly ApplicationDbContext _context;
        public CategoriesService(ApplicationDbContext context)
        {
            _context = context;
        }


        public Category Add(Category categorie)
        {
            _context.Categories.Add(categorie);
            _context.SaveChanges();
            return categorie;
        }
      public   IEnumerable<Category> LoadActive()
        {
            return _context.Categories.Where(a => !a.Deleted).OrderBy(a => a.Name).AsNoTracking().ToList();
        }


        public Category? Find(Expression<Func<Category, bool>> predicate)
        {
            return _context.Categories.FirstOrDefault(predicate);
        }

        public IEnumerable<Category> GetAll(bool asNoTracking = false)
        {
            IQueryable<Category> query = _context.Categories.AsQueryable();
            if (asNoTracking) query.AsNoTracking();
            return query.ToList();
        }

        public Category? GetById(int id)
        => _context.Categories.Find(id);
        

        public void Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }
    }
}
