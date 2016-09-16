﻿
using AssetManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetManagement.WebUI.ViewModel
{
    public class LaptopViewModel
    {
        [Required]
        [Display(Name = "Invoice Number")]
        public string InvoiceNumber { get; set; }
        [Required]
        [Display(Name = "Serial Number")]
        public string serialNumber { get; set; }
        [Required]
        [Display(Name = "Manufacturer")]
        public string manufacturer { get; set; }
        [Required]
        [Display(Name = "Warranty")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Enter numerical values only under warranty")]
        public int warranty { get; set; }
        [Required]
        [DisplayName("Cost Price")]
        public double costprice { get; set; }
        [Required]
        [Display(Name = "Date Added")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dateAdded { get; set; }
        //[Required]
        [Display(Name = "Model")]
        public string modelName { get; set; }
        [Required]
        [Display(Name = "Screen Size")]
        public string screenSize { get; set; }
        [Required]
        [Display(Name = "Operating System")]
        public string OS { get; set; }
        [Required]
        [Display(Name = "RAM")]
        public string RAM { get; set; }
        [Required]
        [Display(Name = "HDD Capacity")]
        public string HDD { get; set; }
    }
}