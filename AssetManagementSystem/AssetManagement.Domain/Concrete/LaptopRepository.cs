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
    public class LaptopRepository : GenericRepository<AssetManagementEntities, Asset, Laptop>, ILaptopRepository
    {
        public Laptop FindAsset(string assetNumber)
        {
            return Context.Laptops.FirstOrDefault(m => m.assetNumber == assetNumber);
        }
        public string AlgorithmAssetID(string serial, string catergory, string manufacturer)
        {
            return (serial.Substring(0, 2) + catergory.Substring(0, 2) + DateTime.Now.Minute + manufacturer.Substring(0, 2)).ToUpper();
        }
        public override void Insert(Asset entity, Laptop dependent)
        {
            entity.catergory = "Laptop";
            dependent.catergory = "Laptop";
            entity.assetNumber = AlgorithmAssetID(entity.serialNumber, entity.catergory, entity.manufacturer);
            dependent.assetNumber = AlgorithmAssetID(dependent.serialNumber, dependent.catergory, dependent.manufacturer);
            entity.assignstatus = 0;
            base.Insert(entity, dependent);
        }
    }
}
