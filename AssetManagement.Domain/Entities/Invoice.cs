using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Entities
{
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceId { get; set; }
        [Required]
        [Display(Name = "Invoice Number")]
        public string InvoiceNumber { get; set; }
        [Required]
        [Display(Name = "Supplier")]
        public string Retailer { get; set; }
        [Required]
        [Display(Name = "Total Cost")]   
        public double totalCost { get; set; }
        [Required]
        [Display(Name ="Quantity")]
        public int Quantity { get; set; }
        public string invoiceType { get; set; }
        [Required]
        public DateTime InvoiceDate { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}
