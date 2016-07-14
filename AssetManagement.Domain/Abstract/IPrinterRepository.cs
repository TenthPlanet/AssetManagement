using AssetManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Abstract
{
    public interface IPrinterRepository : IGenericRepository<Asset, Printer>
    {
        Printer FindAsset(string assetNumber);
        
        string AlgorithmAssetID(string serial, string catergory, string manufacturer);
    }
}
