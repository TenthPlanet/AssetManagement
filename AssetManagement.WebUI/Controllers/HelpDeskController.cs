using AssetManagement.Business.AssetManagement;
using AssetManagement.Business.HelpDeskSystem;
using AssetManagement.Domain.Context;
using AssetManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssetManagement.WebUI.Controllers
{
    public class HelpDeskController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard", "Dashboard");
        }

        [AllowAnonymous]
        public ActionResult Ticket(string id)
        {
            HelpDeskLogic hdl = new HelpDeskLogic();
            AssetManagementLogic aml = new AssetManagementLogic();
            var Ticket = hdl.GetTicket(int.Parse(id));
            var duration = new TimeSpan();
            if (Ticket.accomplishstatus == true)
                duration = Ticket.datecompleted.Value.Subtract(Ticket.datecreated);
            else
                duration = DateTime.Now.Subtract(Ticket.datecreated);
            ViewData["duration"] = duration;
            ViewData["Asset"] = aml.GetAsset(Ticket.assetid.ToString());
            ViewData["ControllerName"] = "HelpDesk";
            Session["Role"] = User.IsInRole("Administrator");
            return View(Ticket);
        }

        [HttpPost]
        public ActionResult Ticket(string id,string solution)
        {
            var _context = new AssetManagementEntities();
            var ticket = _context.Tickets.Find(int.Parse(id));
            ticket.datecompleted = DateTime.Now;
            ticket.accomplishstatus = true;
            ticket.ticketstatus = true;
            ticket.solution = solution;
            _context.Entry(ticket).State = EntityState.Modified;
            _context.SaveChanges();
            TempData["Success"] = "Ticket has been completed";
            return RedirectToAction("Ticket");
        }

        public void comment(string id, string comment)
        {
            if (id != null)
            {
                var _context = new AssetManagementEntities();
                var ticket = _context.Tickets.Find(int.Parse(id));
                var name = _context.Employees.Single(e => e.employeeNumber == User.Identity.Name);
                var progress = new Progress
                {
                    ticketid = ticket.ticketid,
                    comment = comment,
                    date = DateTime.Now,
                    employeeNumber = User.Identity.Name,
                    employeeName = name.fullname
                };
                _context.Progresses.Add(progress);
                _context.SaveChanges();
            }
        }
    }
}