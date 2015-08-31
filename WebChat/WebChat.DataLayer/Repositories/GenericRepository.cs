namespace WebChat.DataLayer.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    public class GenericRepositorty<T> : IGenericRepository<T> where T : class
    {
        private IWebChatContext context;

        public GenericRepositorty(IWebChatContext onlineShopContext)
        {
            this.context = onlineShopContext;
        }
        public IQueryable<T> All()
        {
            return this.context.Set<T>();
        }

        public IQueryable<T> Search(Expression<Func<T, bool>> condition)
        {
            return this.All().Where(condition);
        }

        public T Add(T entity)
        {
            ChangeState(entity, EntityState.Added);
            return entity;
        }

        public T Update(T entity)
        {
            ChangeState(entity, EntityState.Modified);
            return entity;
        }

        public T Delete(T entity)
        {
            ChangeState(entity, EntityState.Deleted);
            return entity;
        }

        public T Detach(T entity)
        {
            ChangeState(entity, EntityState.Detached);
            return entity;
        }

        private void ChangeState(T entity, EntityState state)
        {
            this.context.Set<T>().Attach(entity);
            this.context.Entry(entity).State = state;
        }
    }
}
