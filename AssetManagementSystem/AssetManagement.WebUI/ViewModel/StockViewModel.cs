using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetManagement.WebUI.ViewModel
{
    public class StockViewModel
    {
        [Required]
        [DisplayName("Asset Category")]
        public string Catergory { get; set; }
        [Required]
        [DisplayName("Model")]
        public string Model { get; set; }
        [Required]
        [DisplayName("Manufacturer")]
        public string Manaufacturer { get; set; }
        [Required]
        [DisplayName("Quantity")]
        public string Quantity { get; set; }
    }
}