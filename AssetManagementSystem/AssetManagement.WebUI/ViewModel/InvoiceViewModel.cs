using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetManagement.WebUI.ViewModel
{
    public class InvoiceViewModel
    {
        [Key]
        public int InvoiceId { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        [Required]
        public string Retailer { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public DateTime CaptureDate { get; set; }
    }
}