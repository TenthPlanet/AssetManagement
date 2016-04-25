using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetManagement.WebUI.ViewModel.Asset
{
    public class AssetListViewModel
    {
        public string assetNumber { get; set; }
        public string serialNumber { get; set; }
        public string catergory { get; set; }
        public string manufacturer { get; set; }
        public string warranty { get; set; }
        [DataType(DataType.Date)]
        public DateTime dateadded { get; set; }
        public string assigneddate { get; set; }
        public int assetstatus { get; set; }
        public string costprice { get; set; }

        /*NOT PART OF THE INPUTS*/
        public string owner { get; set; }
        public string depreciationcost { get; set; }
        public string department { get; set; }
        public int assetID { get; set; }
    }
}