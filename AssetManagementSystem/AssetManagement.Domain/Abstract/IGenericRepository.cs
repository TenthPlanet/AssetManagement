using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Abstract
{
    public interface IGenericRepository<T1, T2> 
        where T1 : class 
        where T2 : class
    {
        //IQueryable<T> GetAll();
        void Insert(T1 entity, T2 dependent);
        void Delete(T1 entity, T2 dependent);
        void Update(T1 entity, T2 dependent);
        void Save();

    }
}
