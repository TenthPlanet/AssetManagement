using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Entities
{
    public class Stock
    {
        [Key]
        public int stockID { get; set; }
        public string model { get; set; }
        public string manufacturer { get; set; }
        public string category { get; set; }
        public int quantity { get; set; }
    }
    public class TemporalDevice
    {
        [Key]
        public int TempID { get; set; }
        public int assetID { get; set; }
        public string serialNumber { get; set; }
        public string model { get; set; }
        public string manufacturer { get; set; }
        public string category { get; set; }
        public DateTime dateadded { get; set; }
        public string employeeNumber { get; set; }
        public string employeeFullname { get; set; }

    }
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int catID { get; set; }
        public string category { get; set; }
    }

    public class Asset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int assetID { get; set; }
        public double costprice { get; set; }
        public string assetNumber { get; set; }
        public string serialNumber { get; set; }
        public string catergory { get; set; }
        public string manufacturer { get; set; }
        public string warranty { get; set; }
        public DateTime dateadded { get; set; }
        public DateTime? assigndate { get; set; }
        public int assignstatus { get; set; }
        public double depreciationcost { get; set; }

        public virtual ICollection<Printer> Printers { get; set; }
        public virtual ICollection<PCBox> PCBoxes { get; set; }
        public virtual ICollection<Laptop> Laptops { get; set; }
        public virtual ICollection<Keyboard> Keyboards { get; set; }
        public virtual ICollection<Mouse> Mice { get; set; }
        public virtual ICollection<Monitor> Monitors { get; set; }
        //public virtual ICollection<Catergory> Catergories { get; set; }

        public string employeeNumber { get; set; }
        public virtual Employee Employee { get; set; }
        public string InvoiceNumber { get; set; }
        public List<ReplacementPart> replacementParts { get; set; }
    }

    
    public class Keyboard
    {
        [Key]
        public string assetNumber { get; set; }
        public string serialNumber { get; set; }
        public string manufacturer { get; set; }
        public string modelName { get; set; }
        public string catergory { get; set; }
        public string warranty { get; set; }
        public DateTime dateAdded { get; set; }
        public DateTime? assigndate { get; set; }
        public int assignStatus { get; set; }
        public string employeeNumber { get; set; }
        public virtual Employee Employee { get; set; }
        public int assetID { get; set; }
        public virtual Asset Asset { get; set; }
        public string InvoiceNumber { get; set; }
        public List<ReplacementPart> replacementParts { get; set; }

    }

    public class PCBox 
    {
        [Key]
        public string assetNumber { get; set; }
        public string serialNumber { get; set; }
        public string manufacturer { get; set; }
        public string catergory { get; set; }
        public string warranty { get; set; }
        public DateTime dateAdded { get; set; }
        public DateTime? assigndate { get; set; }
        public int assignStatus { get; set; }
        public string modelName { get; set; }
        public string OS { get; set; }
        public string RAM { get; set; }
        public string HDD { get; set; }

        public string employeeNumber { get; set; }
        public virtual Employee Employee { get; set; }

        public int assetID { get; set; }
        public virtual Asset Asset { get; set; }
        public string InvoiceNumber { get; set; }
        public List<ReplacementPart> replacementParts { get; set; }

    }

    public class Printer 
    {
        [Key]
        public string assetNumber { get; set; }
        public string serialNumber { get; set; }
        public string manufacturer { get; set; }
        public string catergory { get; set; }
        public string warranty { get; set; }
        public DateTime dateAdded { get; set; }
        public DateTime? assigndate { get; set; }
        public int assignStatus { get; set; }
        public string modelName { get; set; }
        public string employeeNumber { get; set; }
        public virtual Employee Employee { get; set; }
        

        public int assetID { get; set; }
        public virtual Asset Asset { get; set; }
        public string InvoiceNumber { get; set; }
        public List<ReplacementPart> replacementParts { get; set; }

    }

    public class Laptop 
    {
        [Key]
        public string assetNumber { get; set; }
        public string serialNumber { get; set; }
        public string manufacturer { get; set; }
        public string catergory { get; set; }
        public string warranty { get; set; }
        public DateTime dateAdded { get; set; }
        public DateTime? assigndate { get; set; }
        public int assignStatus { get; set; }
        public string modelName { get; set; }
        public string screenSize { get; set; }
        public string OS { get; set; }
        public string RAM { get; set; }
        public string processor { get; set; }
        public string HDD { get; set; }

        public string employeeNumber { get; set; }
        public virtual Employee Employee { get; set; }

        public int assetID { get; set; }
        public virtual Asset Asset { get; set; }
        public string InvoiceNumber { get; set; }
        public List<ReplacementPart> replacementParts { get; set; }
    }

    public class Monitor 
    {
        [Key]
        public string assetNumber { get; set; }
        public string serialNumber { get; set; }
        public string manufacturer { get; set; }
        public string catergory { get; set; }
        public string warranty { get; set; }
        public DateTime dateAdded { get; set; }
        public DateTime? assigndate { get; set; }
        public int assignStatus { get; set; }
        public string modelName { get; set; }
        public string displaySize { get; set; }

        public string employeeNumber { get; set; }
        public virtual Employee Employee { get; set; }

        public int assetID { get; set; }
        public virtual Asset Asset { get; set; }
        public string InvoiceNumber { get; set; }
        public List<ReplacementPart> replacementParts { get; set; }

    }

    public class Mouse 
    {
        [Key]
        public string assetNumber { get; set; }
        public string serialNumber { get; set; }
        public string manufacturer { get; set; }
        public string modelName { get; set; }
        public string catergory { get; set; }
        public string warranty { get; set; }
        public DateTime dateAdded { get; set; }
        public DateTime? assigndate { get; set; }
        public int assignStatus { get; set; }

        public string employeeNumber { get; set; }
        public virtual Employee Employee { get; set; }

        public int assetID { get; set; }
        public virtual Asset Asset { get; set; }
        public string InvoiceNumber { get; set; }
        public List<ReplacementPart> replacementParts { get; set; }

    }

    public class Archive
    {
        [Key]
        public string assetNumber { get; set; }
        public string category { get; set; }
        public DateTime dateAdded { get; set; }
        public DateTime dateDisposed { get; set; }
        public string employeeNumber { get; set; }
        public string employeeName { get; set; }
    }

    public class OwnershipHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int historyID { get; set; }
        public string assetNumber { get; set; }
        public string category { get; set; }
        public DateTime assignDate { get; set; }
        public DateTime delocateDate { get; set; }
        public string employeeNumber { get; set; }
        public string employeeFullname { get; set; }
    }
}
