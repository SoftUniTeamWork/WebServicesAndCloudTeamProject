using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebChat.DataLayer.Contracts;

namespace WebChat.Tests.MockedObjects
{
    public class GenericRepositoryMock<T> : IGenericRepository<T>
        where T : class
    {
        public IList<T> Entities { get; set; }

        public GenericRepositoryMock()
        {
            this.Entities = new List<T>();
        }

        public IQueryable<T> GetAll()
        {
            return this.Entities.AsQueryable();
        }

        public T GetById(object id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Search(System.Linq.Expressions.Expression<Func<T, bool>> condition)
        {
            throw new NotImplementedException();
        }

        public T Add(T entity)
        {
            this.Entities.Add(entity);

            return entity;
        }

        public T Update(T entity)
        {
            throw new NotImplementedException();
        }

        public T Delete(T entity)
        {
            this.Entities.Remove(entity);

            return entity;
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public T Detach(T entity)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            this.IsSaveCalled = true;
        }

        public bool IsSaveCalled { get; set; }
    }
}
