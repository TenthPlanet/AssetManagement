using AssetManagement.Domain.Abstract;
using AssetManagement.Domain.Context;
using AssetManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Concrete
{
    public class TicketRepository : GenericEntity<AssetManagementEntities, Ticket>, ITicketRepository
    {
        public Ticket FindTicket(int id)
        {
            return _entities.Tickets.Find(id);
        }
    }
}
