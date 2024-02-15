﻿using Bookify.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        public IBaseRepository<Author> Authors { get;  }

        int Commit();
    }
}
