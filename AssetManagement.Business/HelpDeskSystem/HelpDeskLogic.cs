using AssetManagement.Domain.Entities;
using AssetManagement.Domain.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Business.HelpDeskSystem
{
    public class HelpDeskLogic
    {
        public List<Ticket> allTickets;
        public List<Ticket> closedTickets;
        public List<Ticket> openTickets;
        public List<Ticket> aknowlagedTickets;
        public List<Ticket> unAknowlagedTickets;
        private readonly AssetManagementEntities _context = new AssetManagementEntities();
        

        public HelpDeskLogic()
        {
            allTickets = _context.Tickets.ToList();
            closedTickets = _context.Tickets.Where(t => t.accomplishstatus == true).ToList();
            openTickets = _context.Tickets.Where(t => t.accomplishstatus == false).ToList();
            aknowlagedTickets = _context.Tickets.Where(t => t.acknowledgestatus == true).ToList();
            unAknowlagedTickets = _context.Tickets.Where(t => t.acknowledgestatus == false).ToList();
        }

        public List<Ticket> CompletedTickets(Employee TicketParticipant)
        {
            return closedTickets.Where(emp => emp.employeeNumber == TicketParticipant.employeeNumber).ToList();
        }
        public List<Ticket> OpenTickets(Employee TicketParticipant)
        {
            return openTickets.Where(emp => emp.employeeNumber == TicketParticipant.employeeNumber).ToList();
        }
        public List<Ticket> AknowlagedTickets(Employee TicketParticipant)
        {
            return aknowlagedTickets.Where(emp => emp.employeeNumber == TicketParticipant.employeeNumber).ToList();
        }
        public List<Ticket> UnAknowlagedTickets(Employee TicketParticipant)
        {
            return unAknowlagedTickets.Where(emp => emp.employeeNumber == TicketParticipant.employeeNumber).ToList();
        }
        public List<Ticket> AllTickets(Employee TicketParticipant)
        {
            return allTickets.Where(emp => emp.employeeNumber == TicketParticipant.employeeNumber).ToList();
        }

        public List<Employee> AllAdministrators()
        {
            return _context.Employees.Where(emp => emp.position == "Administator").ToList();
        }
        public List<Employee> AllTechnicians()
        {
            return _context.Employees.Where(emp => emp.position == "Technician").ToList();
        }

        public IEnumerable<Tech> TechnicianStats()
        {
            List<Tech> participants = new List<Tech>();
            foreach(var participant in AllTechnicians())
            {
                var returnParticipant = new Tech()
                {
                    Name = participant.fullname,
                    AllTickets = AllTickets(participant).Count,
                    OpenedTickets = OpenTickets(participant).Count,
                    CompletedTickets = CompletedTickets(participant).Count,
                    UnAcknowalgedTickets = UnAknowlagedTickets(participant).Count
                };
                participants.Add(returnParticipant);
            }
            return participants;
        }
        public IEnumerable<Admin> AdministratorStats()
        {
            List<Admin> participants = new List<Admin>();
            foreach (var participant in AllAdministrators())
            {
                var returnParticipant = new Admin()
                {
                    Name = participant.fullname,
                    AllTickets = AllTickets(participant).Count,
                    OpenedTickets = OpenTickets(participant).Count,
                    CompletedTickets = CompletedTickets(participant).Count,
                    UnAcknowalgedTickets = UnAknowlagedTickets(participant).Count
                };
                participants.Add(returnParticipant);
            }
            return participants;
        }
    }
}
