


using System.Linq.Expressions;

namespace Bookify.Infrastructure.Services;

    internal class AuthorsService:IAuthorsService
    {
        private readonly ApplicationDbContext _context;
    public AuthorsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Author> GetAll( bool asNoTracking = false) 
    {
            IQueryable<Author> query = _context.Authors.AsQueryable();
            if(asNoTracking)   query.AsNoTracking(); 
            return query.ToList();
    }

  public   IEnumerable<Author> LoadActive()
    {
      return   _context.Authors.Where(a => !a.Deleted).OrderBy(a => a.Name).AsNoTracking().ToList();
    }
    public  Author Add(Author author)
    {
        _context.Authors.Add(author);
        _context.SaveChanges();
        return author;
    }
    public Author? GetById(int id) 
    { 
        return _context.Authors.Find(id);
    }
   public  void Update(Author author)
    {
        _context.Authors.Update(author);
        _context.SaveChanges();
    }
   public  Author? Find(Expression<Func<Author, bool>> predicate)
    {
        return _context.Authors.SingleOrDefault(predicate);
    }

}

