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
        public List<Ticket> ClosedTickets(Employee TicketParticipant)
        {
            return closedTickets.Where(emp => emp.employeeNumber == TicketParticipant.employeeNumber).ToList();
        }

        public List<Employee> AllAdministrators()
        {
            return _context.Employees.Where(emp => emp.position == "Administator").ToList();
        }
        public List<Employee> AllTechnicians()
        {
            return _context.Employees.Where(emp => emp.position == "Technician").ToList();
        }

        public IEnumerable<TicketParticipant> TechnicianStats()
        {
            var participants = new List<TicketParticipant>();
            foreach (var participant in AllTechnicians())
            {
                var returnParticipant = new TicketParticipant()
                {
                    Name = participant.fullname,
                    employeeID = participant.employeeNumber,
                    AllTickets = AllTickets(participant).Count,
                    OpenedTickets = OpenTickets(participant),
                    CompletedTickets = CompletedTickets(participant).Count,
                    UnAcknowalgedTickets = UnAknowlagedTickets(participant).Count
                };
                participants.Add(returnParticipant);
            }
            return participants;
        }
        public IEnumerable<TicketParticipant> AdministratorStats()
        {
            var participants = new List<TicketParticipant>();
            foreach (var participant in AllAdministrators())
            {
                var returnParticipant = new TicketParticipant()
                {
                    Name = participant.fullname,
                    employeeID = participant.employeeNumber,
                    AllTickets = AllTickets(participant).Count,
                    OpenedTickets = OpenTickets(participant),
                    CompletedTickets = CompletedTickets(participant).Count,
                    UnAcknowalgedTickets = UnAknowlagedTickets(participant).Count
                };
                participants.Add(returnParticipant);
            }
            return participants;
        }
        public Employee GetEmployee(string id)
        {
            AssetManagementEntities ams = new AssetManagementEntities();
            return ams.Employees.Find(id);
        }

        public List<Ticket> TicketsFilter(status status)
        {
            var tickets = new List<Ticket>();
            switch (status)
            {
                case status.opened:
                    tickets = openTickets;
                    break;
                case status.completed:
                    tickets = closedTickets;
                    break;
                case status.unAknowlaged:
                    tickets = unAknowlagedTickets;
                    break;
                case status.all:
                    tickets = allTickets;
                    break;
            }
            return tickets;
        }
        //public object TicketsFilter(string employeeId, status opened)
        //{
        //    var employee = GetEmployee(employeeId);
        //    var tickets = new List<Ticket>();
        //    switch (opened)
        //    {
        //        case status.opened:
        //            tickets = OpenTickets(employee);
        //            break;
        //        case status.closed:
        //            tickets = ClosedTickets(employee);
        //            break;
        //        case status.completed:
        //            tickets = CompletedTickets(employee);
        //            break;
        //        case status.unAknowlaged:
        //            tickets = UnAknowlagedTickets(employee);
        //            break;
        //        case status.all:
        //            tickets = AllTickets(employee);
        //            break;
        //    }
        //    return tickets;
        //}
    }
    public enum status
    {
        opened = 0,
        unAknowlaged,
        completed,
        all
    }
}
