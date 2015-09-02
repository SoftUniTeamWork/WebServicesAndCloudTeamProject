using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using WebChat.DataLayer.Contracts;

namespace WebChat.DataLayer.Repositories
{
    public class GenericRepositorty<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext context;
        private readonly IDbSet<T> set; 

        public GenericRepositorty(DbContext onlineShopContext)
        {
            this.context = onlineShopContext;
            this.set = context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return this.context.Set<T>();
        }

        public IQueryable<T> Search(Expression<Func<T, bool>> condition)
        {
            return this.GetAll().Where(condition);
        }


        public void Dispose()
        {
            this.context.Dispose();
        }

        public T GetById(object id)
        {
            return this.set.Find(id);
        }

        public T Add(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Added);
            
            return entity;
        }

        public T Update(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Modified);

            return entity;
        }

        public T Delete(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Deleted);

            return entity;
        }

        public void Delete(object id)
        {
            this.Delete(this.GetById(id));
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private void ChangeEntityState(T entity, EntityState state)
        {
            var entry = this.context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.set.Attach(entity);
            }

            entry.State = state;
        }

        public T Detach(T entity)
        {
            ChangeEntityState(entity, EntityState.Detached);
            return entity;
        }
    }
}
