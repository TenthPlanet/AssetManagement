using System.Collections.Generic;
using AssetManagement.Domain.Entities;

namespace AssetManagement.WebUI.Controllers
{
    internal class FinencialReport
    { 
        public List<Invoice> invoices { get; set; }
        public List<ReplacementPart> replacementParts { get; set; }
        public double TotalAssetCost { get; set; }
        public double TotalSparePartsCost { get; set; }
    }
}