namespace WebChat.DataLayer.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using Contracts;
    using System.Data.Entity.Infrastructure;

    public class GenericRepositorty<T> : IGenericRepository<T> where T : class
    {
        private readonly IWebChatContext context;

        public GenericRepositorty(IWebChatContext onlineShopContext)
        {
            this.context = onlineShopContext;
        }

        public GenericRepositorty()
            :this(new WebChatContext())
        {
            
        }
        public IQueryable<T> GetAll()
        {
            return this.context.Set<T>();
        }

        public IQueryable<T> Search(Expression<Func<T, bool>> condition)
        {
            return this.GetAll().Where(condition);
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
           var entry = this.AttachIfDetached(entity);
           entry.State = state;
       }

       private DbEntityEntry AttachIfDetached(T entity)
       {
           var entry = this.context.Entry(entity);

           if (entry.State == EntityState.Detached)
           {
               this.context.Set<T>().Attach(entity);
           }

           return entry;
       }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        
    }
}
