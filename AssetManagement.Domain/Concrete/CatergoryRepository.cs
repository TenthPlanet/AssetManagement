using AssetManagement.Domain.Abstract;
using AssetManagement.Domain.Context;
using AssetManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Concrete
{
    public class CatergoryRepository : GenericEntity<AssetManagementEntities, Category>, ICatergoryRepository
    {
        public List<Category> Catergories()
        {
            return _entities.Categories.ToList();
        }
    }
}
