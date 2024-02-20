using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Services;

    public  interface IAuthorsService
    {
    IEnumerable<Author> GetAll(bool asNoTracking = false);
    Author Add (Author author);
    Author? GetById(int id);
    void Update(Author author);
    Author? Find(Expression<Func<Author, bool>> predicate);
    }

