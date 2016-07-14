using AssetManagement.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Concrete
{
    public class GenericEntity<C, T1> : IGenericEntity<T1>
        where T1 : class
        where C : DbContext, new()
    {
        public C _entities = new C();

        protected C Context
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public virtual void Insert(T1 entity)
        {
            _entities.Set<T1>().Add(entity);
        }

        public virtual void Delete(T1 entity)
        {
            _entities.Set<T1>().Remove(entity);
        }

        public virtual void Update(T1 entity)
        {
            _entities.Entry(entity).State = EntityState.Modified;

        }

        public virtual void Save()
        {
            _entities.SaveChanges();
        }
    }
}
