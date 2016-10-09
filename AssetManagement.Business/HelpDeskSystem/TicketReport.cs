using AssetManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Business.HelpDeskSystem
{
    public class TicketReportPerParticipant
    {
        public List<Ticket> Opened { get; set; }
        public List<Ticket> UnAcknowlaged { get; set; }
        public List<Ticket> Completed { get; set; }
        public List<Ticket> All { get; set; }
    }
    public class TicketReport
    {
        public List<Ticket> Opened { get; set; }
        public List<Ticket> UnAknowlaged { get; set; }
        public List<Ticket> Completed { get; set; }
        public List<Ticket> All { get; set; }
    }

}
