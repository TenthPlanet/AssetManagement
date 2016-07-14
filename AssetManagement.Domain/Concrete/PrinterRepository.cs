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
    public class PrinterRepository : GenericRepository<AssetManagementEntities, Asset, Printer>, IPrinterRepository
    {
        public Printer FindAsset(string assetNumber)
        {
            return Context.Printers.FirstOrDefault(m => m.assetNumber == assetNumber);
        }
        public string AlgorithmAssetID(string serial, string catergory, string manufacturer)
        {
            return (serial.Substring(0, 2) + catergory.Substring(0, 2) + DateTime.Now.Minute + manufacturer.Substring(0, 2)).ToUpper();
        }

        public override void Insert(Asset entity, Printer dependent)
        {
            entity.catergory = "Printer";
            dependent.catergory = "Printer";
            entity.assetNumber = AlgorithmAssetID(entity.serialNumber, entity.catergory, entity.manufacturer);
            dependent.assetNumber = AlgorithmAssetID(dependent.serialNumber, dependent.catergory, dependent.manufacturer);
            entity.assignstatus = 0;
            dependent.assignStatus = 0;
            base.Insert(entity, dependent);
        }
    }
}
