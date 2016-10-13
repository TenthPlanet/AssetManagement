using AssetManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AssetManagement.Business
{
    public class AssetLogic
    {
        
        public double depreciationCost(DateTime dateadded, double cost)
        {
            double deprcost = 0;
            if ((DateTime.Now.Year - dateadded.Year) >= 1)
            {
                deprcost = cost - ((cost * 0.3) * (DateTime.Now.Year - dateadded.Year));
            }
            if (cost - ((cost * 0.3) * (DateTime.Now.Year - dateadded.Year)) < 0)
            {
                deprcost = cost;
            }
            return deprcost;
        }
        public byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            byte[] ImageBytes = null;
            BinaryReader reader = new BinaryReader(file.InputStream);
            ImageBytes = reader.ReadBytes((int)file.ContentLength);
            return ImageBytes;
        }

    }
}
