using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetManagement.WebUI.ViewModel
{
    public class TicketParticipantView
    {
        public string Name { get; set; }
        public int OpenedTickets { get; set; }
        public int CompletedTickets { get; set; }
        public int UnAcknowalgedTickets { get; set; }
        public int AllTickets { get; set; }
    }
}