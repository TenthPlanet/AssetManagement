using AssetManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.WebUI.ViewModel
{
    public class TicketViewModel
    {
        public int ticketid { get; set; }
        [DisplayName("Asset #")]
        public string assetnumber { get; set; }
        public int assetid { get; set; } //This is so I can easily get an asset report
        [DisplayName("Employee #")]
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
        [DisplayName("Full Name")]
        public string fullname { get; set; }
    }

    public class GeneralTicketViewModel
    {
        public int ticketid { get; set; }
        [DisplayName("Employee #")]
        public string employee { get; set; }
        [DisplayName("Priority")]
        public string priority { get; set; }
        [DisplayName("Subject")]
        public string subject { get; set; }
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
        [DisplayName("Full Name")]
        public string fullname { get; set; }

        public string employeeNumber { get; set; }
    }

    public class ContactUsViewModel
    {
        public int id { get; set; }
        [Required]
        public string subject { get; set; }
        [Required]
        public string body { get; set; }
        public string username { get; set; }
        public bool read { get; set; }
        public DateTime datesent { get; set; }
        public string catergory { get; set; }
    }
    public class ScreenshotsViewModel
    {
        public int id { get; set; }
        public byte[] imagedata { get; set; }
        public string imagemimeType { get; set; }
    }
}
