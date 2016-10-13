using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Entities
{
    public class Ticket
    {
        [Key]
        public int ticketid { get; set; }
        [DisplayName("Asset #")]
        public string assetnumber { get; set; }
        public int assetid { get; set; } //This is so I can easily get an asset report
        [DisplayName("Employee #")]
        public string assetowner { get; set; }
        [DisplayName("Category")]
        public string category { get; set; }
        [DisplayName("Subject")]
        public string subject { get; set; }
        [DisplayName("Priority")]
        public string priority { get; set; }
        [DisplayName("Description")]
        public string description { get; set; }
        public bool accomplishstatus { get; set; }
        public bool acknowledgestatus { get; set; }
        public bool ticketstatus { get; set; }
        [DataType(DataType.Date)]
        public DateTime datecreated { get; set; }
        [DisplayName("Due Date")]
        [DataType(DataType.Date)]
        public DateTime datedue { get; set; }
        public DateTime? datecompleted { get; set; }
        [Display(Name = "Solution")]
        public string solution { get; set; }
        public string employeeNumber { get; set; }
        public virtual Employee Employees { get; set; }
        public ICollection<Progress> Progresses { get; set; }
        public List<int> replacementParts { get; set; }
    }

    public class Progress
    {
        [Key]
        public int progessID { get; set; }
        public string comment { get; set; }
        [DataType(DataType.Date)]
        public DateTime? date { get; set; }
        public int ticketid { get; set; }
        public string employeeName { get; set; }
        public virtual Ticket Tickets { get; set; }
        public string employeeNumber { get; set; }
        public virtual Employee Employee { get; set; }

    }
    public class ContactUs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int contactId { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string userName { get; set; }
        [DefaultValue(false)]
        public bool read { get; set; }
        public DateTime datesent { get; set; }
        public string category { get; set; }
        public ICollection<Screenshot> screenshots { get; set; }

    }
    public class Screenshot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string filename { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        public int contactId { get; set; }
        public virtual ContactUs contactUs { get; set; }
    }
    public class ReplacementPart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReplacementID { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Assciated Asset")]
        public string AssociatedtedAsset { get; set; }
        [Display(Name = "Assciated Ticket")]
        public int AssociatedTicket { get; set; }
        [Display(Name = "Invoice Number")]
        public string InvoiceNumber { get; set; }
        [Display(Name = "Supplier")]
        public string Supplier { get; set; }
        [Display(Name = "Price")]
        public double Price { get; set; }
        [Display(Name = "Part Number")]
        public string PartNumber { get; set; }
        [Display(Name = "Purchase Date")]
        public DateTime PurchaseDate { get; set; }

    }
}
