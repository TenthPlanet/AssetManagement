using AssetManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
