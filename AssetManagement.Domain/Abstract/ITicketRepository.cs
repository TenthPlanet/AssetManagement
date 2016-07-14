using AssetManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Abstract
{
    public interface ITicketRepository : IGenericEntity<Ticket>
    {
        Ticket FindTicket(int id);
    }
}
