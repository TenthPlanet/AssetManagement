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
        public double TotalSparePartsCost;
        public double TotalAssetCost;
        public List<Invoice> invoices { get; set; }
        public List<Asset> assets { get; set; }
        public List<ReplacementPart> replacementParts { get; set; }
        public double TotalCost;
        public double qty;
    }
}
