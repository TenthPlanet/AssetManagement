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
        public string employeeID { get; set; }
        public List<Ticket> OpenedTickets { get; set; }
        public List<Ticket> CompletedTickets { get; set; }
        public List<Ticket> UnAcknowalgedTickets { get; set; }
        public List<Ticket> AllTickets { get; set; }   
        
        public TicketParticipant()
        {

        } 
        public TicketParticipant(string Name, int EmployeeID,  List<Ticket> opened, List<Ticket> completed,  List<Ticket> UbAckowlaged,  List<Ticket> All)
        {
            this.Name = Name;
            this.OpenedTickets = opened;
            this.CompletedTickets = completed;
            this.UnAcknowalgedTickets = UnAcknowalgedTickets;
            this.AllTickets = All;
        }
    }
}
