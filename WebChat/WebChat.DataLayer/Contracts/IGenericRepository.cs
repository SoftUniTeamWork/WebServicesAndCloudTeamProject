namespace WebChat.DataLayer.Contracts
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IGenericRepository<T>
    {
        IQueryable<T> GetAll();

        IQueryable<T> Search(Expression<Func<T, bool>> condition);

        T Add(T entity);

        T Update(T entity);

        T Delete(T entity);

        T Detach(T entity);

        void Save();
    }
}
