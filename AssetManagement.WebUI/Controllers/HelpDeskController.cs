using AssetManagement.Business.AssetManagement;
using AssetManagement.Business.HelpDeskSystem;
using AssetManagement.Domain.Context;
using AssetManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            var TicketAsset = aml.GetAsset(Ticket.assetid.ToString());
            var duration = new TimeSpan();
            if (Ticket.accomplishstatus == true)
                duration = Ticket.datecompleted.Value.Subtract(Ticket.datecreated);
            else
                duration = DateTime.Now.Subtract(Ticket.datecreated);
            ViewData["duration"] = duration;
            ViewData["Asset"] = TicketAsset;
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
        [AllowAnonymous]
        public ActionResult TicketDetails(int? id)
        {
            var _context = new AssetManagementEntities();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ticket = _context.Tickets.Find(id);
            var progress = _context.Progresses.Where(x => x.ticketid == ticket.ticketid);
            var model = new Tuple<Ticket, IEnumerable<Progress>>(ticket, progress);

            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [AllowAnonymous]
        [HttpPost, ActionName("TicketDetails")]
        [ValidateAntiForgeryToken]
        public ActionResult WriteComment(int? id, string comment)
        {
            var _context = new AssetManagementEntities();
            if (id != null)
            {
                var ticket = _context.Tickets.Find(id);
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
                return RedirectToAction("Ticket", new { id = ticket.ticketid.ToString() });
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult RenderImage(string id)
        {
            var _context = new AssetManagementEntities();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var image = _context.Employees.FirstOrDefault(x => x.employeeNumber == id);

            if (image == null)
            {
                return HttpNotFound();
            }

            return File(image.fileBytes, image.fileType);
        }

    }
}