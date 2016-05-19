using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Entities
{
    public class Ticket
    {
        [Key]
        public int ticketid { get; set; }
        [DisplayName("Asset No.")]
        public string assetnumber { get; set; }
        public int assetid { get; set; } //This is so I can easily get an asset report
        [DisplayName("Employee No.")]
        public string assetowner { get; set; }
        [DisplayName("Subject")]
        public string subject { get; set; }
        [DisplayName("Priority")]
        public string priority { get; set; }
        [DisplayName("Description")]
        public string description { get; set; }
        //public int numOfEscalatedTimes { get; set; }
        public bool accomplishstatus { get; set; }
        public bool acknowledgestatus { get; set; }
        public bool ticketstatus { get; set; }
        [DataType(DataType.Date)]
        public DateTime datecreated { get; set; }
        [DisplayName("Due Date")]
        [DataType(DataType.Date)]
        public DateTime datedue { get; set; }
        
        public string employeeNumber { get; set; }
        public virtual Employee Employees { get; set; }
    }
}
