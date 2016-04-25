using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetManagement.WebUI.ViewModel
{
    public class PrinterViewModel
    {
        [Required]
        [DisplayName("Serial Number")]
        public string serialNumber { get; set; }
        [Required]
        [DisplayName("Manufacturer")]
        public string manufacturer { get; set; }
        [Required]
        [DisplayName("Model")]
        public string modelName { get; set; }
        [Required]
        [DisplayName("Warranty")]
        public string warranty { get; set; }
        [Required]
        [DisplayName("Cost Price")]
        public double costprice { get; set; }
        [Required]
        [DisplayName("Date Added")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dateAdded { get; set; }
    }
}