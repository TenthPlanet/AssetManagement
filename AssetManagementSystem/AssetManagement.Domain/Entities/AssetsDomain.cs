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
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int catID { get; set; }
        public string category { get; set; }
    }
    [Table("Asset")]
    public abstract class Asset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int assetID { get; set; }
        public double costprice { get; set; }
        public string assetNumber { get; set; }
        public string serialNumber { get; set; }
        public string catergory { get; set; }
        public string manufacturer { get; set; }
        public string modelName { get; set; }
        public string warranty { get; set; }
        public DateTime dateAdded { get; set; }
        public DateTime? assigndate { get; set; }
        public int assignstatus { get; set; }
        public int assignStatus { get; set; }
        public string employeeNumber { get; set; }
        public virtual Employee Employee { get; set; }
    }
    [Table("Keyboard")]
    public class Keyboard : Asset
    {}
    [Table("PCBox")]
    public class PCBox : Asset
    {    
        public string OS { get; set; }
        public string RAM { get; set; }
        public string HDD { get; set; }
    }
    [Table("Printer")]
    public class Printer : Asset
    {}
    [Table("Laptop")]
    public class Laptop : Asset
    {      
        public string screenSize { get; set; }
        public string OS { get; set; }
        public string RAM { get; set; }
        public string HDD { get; set; }
    }
    [Table("Monitor")]
    public class Monitor : Asset
    {
        public string displaySize { get; set; }
    }
    [Table("Mouse")]
    public class Mouse : Asset
    {}
    [Table("Archive")]
    public class Archive : Asset
    {
        [Key]
        public string category { get; set; }
        public DateTime dateDisposed { get; set; }
        public string employeeName { get; set; }
    }
    [Table("OwnershipHistory")]
    public class OwnershipHistory : Asset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int historyID { get; set; }
        public DateTime delocateDate { get; set; }
        public string employeeFullname { get; set; }
    }
}
