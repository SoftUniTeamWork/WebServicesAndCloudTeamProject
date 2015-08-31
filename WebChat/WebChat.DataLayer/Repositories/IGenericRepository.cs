﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace WebChat.DataLayer.Repositories
{
    public interface IGenericRepository<T>
    {
        IQueryable<T> All();

        IQueryable<T> Search(Expression<Func<T, bool>> condition);

        T Add(T entity);

        T Update(T entity);

        T Delete(T entity);

        T Detach(T entity);
    }
}