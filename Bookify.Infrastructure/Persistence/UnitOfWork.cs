using Bookify.Infrastructure.Repositories;


namespace Bookify.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
                _context = context;
        }
        public IBaseRepository<Author> Authors => new BaseRepository<Author>(_context);

        public int Commit()
        {
            return _context.SaveChanges();
        }
    }
}
