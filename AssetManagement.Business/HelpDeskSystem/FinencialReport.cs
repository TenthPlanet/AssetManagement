using AssetManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Business.HelpDeskSystem
{
    public class FinencialReport
    {
        public int AssetsBought { get; set; }
        public int ReplacementPartsBought { get; set; }
        public double TotalRepairCost { get; set; }
        public double TotalAssetsCost { get; set; }
        public double FullCost { get; set; }
    }
}
