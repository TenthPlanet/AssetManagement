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
    public class StockRepository : GenericEntity<AssetManagementEntities, Stock>, IStockRepository
    {
        public List<Stock> Stocks()
        {
            return _entities.Stocks.ToList();
        }
        public override void Insert(Stock entity)
        {
            base.Insert(entity);
        }
    }
}
