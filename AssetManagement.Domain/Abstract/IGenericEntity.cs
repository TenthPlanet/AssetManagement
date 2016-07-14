using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Abstract
{
    public interface IGenericEntity<T1> where T1 : class
    {
        void Insert(T1 entity);
        void Delete(T1 entity);
        void Update(T1 entity);
        void Save();
    }
}
