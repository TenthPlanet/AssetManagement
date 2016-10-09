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
        public List<ReplacementPart> spareParts;
        public List<Asset> assets;
        public List<Stock> stocks;

        public List<Invoice> invoices { get; set; }

        private readonly AssetManagementEntities _context = new AssetManagementEntities();
        public HelpDeskLogic()
        {
            allTickets = _context.Tickets.ToList();
            closedTickets = _context.Tickets.Where(t => t.accomplishstatus == true).ToList();
            openTickets = _context.Tickets.Where(t => t.accomplishstatus == false).ToList();
            aknowlagedTickets = _context.Tickets.Where(t => t.acknowledgestatus == true).ToList();
            unAknowlagedTickets = _context.Tickets.Where(t => t.acknowledgestatus == false).ToList();
            spareParts = _context.ReplacementParts.ToList();
            invoices = _context.Invoices.ToList();
            assets = _context.Assets.ToList();
            stocks = _context.Stocks.ToList();
        }

        public List<Ticket> CompletedTickets(string id)
        {
            return closedTickets.Where(emp => emp.employeeNumber == id).ToList();
        }
        public List<Ticket> OpenTickets(string id)
        {
            return openTickets.Where(emp => emp.employeeNumber == id).ToList();
        }
        public List<Ticket> AknowlagedTickets(string id)
        {
            return aknowlagedTickets.Where(emp => emp.employeeNumber == id).ToList();
        }
        public List<Ticket> UnAknowlagedTickets(string id)
        {
            return unAknowlagedTickets.Where(emp => emp.employeeNumber == id).ToList();
        }
        public List<Ticket> AllTickets(string id)
        {
            return allTickets.Where(emp => emp.employeeNumber == id).ToList();
        }

        public List<Employee> AllAdministrators()
        {
            return _context.Employees.Where(emp => emp.position == "Administrator").ToList();
        }
        public List<Employee> AllTechnicians()
        {
            return _context.Employees.Where(emp => emp.position == "Technician").ToList();
        }

        public Ticket GetTicket(int id)
        {
            return allTickets.Find(t => t.ticketid == id);
        }

        public void AddReplacementPart(ReplacementPart part)
        {
            _context.ReplacementParts.Add(part);
            _context.SaveChanges();
        }
        public Employee GetEmployee(string id)
        {
            AssetManagementEntities ams = new AssetManagementEntities();
            return ams.Employees.Find(id);
        }

        public TicketReportPerParticipant GetParticipantReport(string id)
        {
            TicketReportPerParticipant participant = new TicketReportPerParticipant();
            participant.Opened = OpenTickets(id);
            participant.Completed = CompletedTickets(id);
            participant.UnAcknowlaged = UnAknowlagedTickets(id);
            participant.All = AllTickets(id);
            return participant;
        }
        public TicketReportPerParticipant getAllTickets()
        {
            var tr = new TicketReportPerParticipant();
            tr.All = allTickets;
            tr.Completed = closedTickets;
            tr.Opened = openTickets;
            tr.UnAcknowlaged = unAknowlagedTickets;

            return tr;
        }

        public List<Ticket> GetOverDueTickets()
        {
            var overDueTickets = new List<Ticket>();

            overDueTickets = allTickets.Where(t => t.datedue > DateTime.Now)
                .Where(t => t.accomplishstatus==false).ToList();
            return overDueTickets;
        }
        public double TotalPartsCost()
        {
            double total = 0;
            foreach (var part in spareParts)
            {
                total += part.price;
            }
            return total;
        }
        public double TotalAssetsCost()
        {
            double total = 0;
            foreach (var part in invoices)
            {
                if (part.invoiceType == "a")
                    total += part.totalCost;
                else
                    continue;
            }
            return total;
        }
        public double Qunatity()
        {
            double qty = 0;
            foreach (var stock in stocks)
            {
                qty += stock.quantity;
            }
            return qty;
        }
        public double TotalCost()
        {
            return TotalAssetsCost() + TotalPartsCost();
        }
    }
}
