using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.WebUI.ViewModel.Asset
{
    public class AssetReport
    {
        public int assetID { get; set; }
        public string assetNumber { get; set; }
        public string serialNumber { get; set; }
        public string catergory { get; set; }
        public string manufacturer { get; set; }
        public string warranty { get; set; }
        [DataType(DataType.Date)]
        public DateTime dateadded { get; set; }
        [DataType(DataType.Date)]
        public DateTime? assigneddate { get; set; }
        public string sellprice { get; set; }
        public int assetstatus { get; set; }
        public string costprice { get; set; }

        public string owner { get; set; }
        public string depreciationcost { get; set; }
    }
}
