using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Entities
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        [Required]
        [Display(Name = "Invoice Number")]
        public string InvoiceNumber { get; set; }
        [Required]
        [Display(Name = "Supplier")]
        public string Retailer { get; set; }
        public DateTime CaptureDate { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public byte[] Content { get; set; }

    }
}
