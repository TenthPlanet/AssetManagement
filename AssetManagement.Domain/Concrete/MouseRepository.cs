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
    public class MouseRepository : GenericRepository<AssetManagementEntities, Asset, Mouse>, IMouseRepository
    {

        public Mouse FindAsset(string assetNumber)
        {
            return Context.Mice.FirstOrDefault(x => x.assetNumber == assetNumber);
        }

        public string AlgorithmAssetID(string serial, string catergory, string manufacturer)
        {
            return (serial.Substring(0, 2) + catergory.Substring(0, 2) + DateTime.Now.Minute + manufacturer.Substring(0, 2)).ToUpper();
        }
        public override void Insert(Asset entity, Mouse dependent)
        {
            entity.catergory = "Mouse";
            dependent.catergory = entity.catergory;
            entity.assetNumber = AlgorithmAssetID(entity.serialNumber, entity.catergory, entity.manufacturer);
            dependent.assetNumber = entity.assetNumber;
            entity.assignstatus = 0;
            dependent.assignStatus = entity.assignstatus;
            base.Insert(entity, dependent);
        }
    }
}
