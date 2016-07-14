using AssetManagement.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Concrete
{
    public class GenericRepository<C, T1, T2> : IGenericRepository<T1, T2>
        where T1 : class
        where T2 :class
        where C : DbContext, new()
        
    {
      
        public C _entities = new C();

        protected C Context
        {
            get { return _entities; }
            set { _entities = value; }
        }
        //public virtual IQueryable<T> GetAll()
        //{
        //    IQueryable<T> query = _entities.Set<T>();
        //    return query;
        //}

        public virtual void Insert(T1 entity, T2 dependent)
        {
            _entities.Set<T1>().Add(entity);
            _entities.Set<T2>().Add(dependent);
        }

        public virtual void Delete(T1 entity, T2 dependent)
        {
            _entities.Set<T1>().Remove(entity);
            _entities.Set<T2>().Remove(dependent);
        }

        public virtual void Update(T1 entity, T2 dependent)
        {
            _entities.Entry(entity).State = EntityState.Modified;
            _entities.Entry(dependent).State = EntityState.Modified;

        }

        public virtual void Save()
        {
            _entities.SaveChanges();
        }
    }
}
