using AssetManagement.Domain.Context;
using AssetManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Business.HelpDeskSystem
{
    public class TicketParticipant
    {
        public string Name { get; set; }
        public int OpenedTickets { get; set; }
        public int CompletedTickets { get; set; }
        public int UnAcknowalgedTickets { get; set; }
        public int AllTickets { get; set; }    
    }
    public class Tech : TicketParticipant
    {

    }
    public class Admin : TicketParticipant
    {

    }
}
