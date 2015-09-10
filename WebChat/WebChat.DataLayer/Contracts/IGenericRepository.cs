namespace WebChat.DataLayer.Contracts
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IGenericRepository<T>
    {
        IQueryable<T> GetAll();

        T GetById(object id);

        IQueryable<T> Search(Expression<Func<T, bool>> condition);

        T Add(T entity);

        T Update(T entity);

        T Delete(T entity);

        void Delete(object id);

        T Detach(T entity);

        void SaveChanges();
    }
}
