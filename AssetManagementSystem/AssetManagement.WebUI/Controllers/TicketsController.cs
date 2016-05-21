using AssetManagement.Domain.Abstract;
using AssetManagement.Domain.Concrete;
using AssetManagement.Domain.Context;
using AssetManagement.Domain.Entities;
using AssetManagement.WebUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AssetManagement.WebUI.Controllers
{
    public class TicketsController : Controller
    {
        public TicketsController() 
            : this(new TicketRepository())
        { }

        private readonly ITicketRepository _repository;
        private readonly AssetManagementEntities _context = new AssetManagementEntities();

        public TicketsController(ITicketRepository _repository)
        {
            this._repository = _repository;
        }

        // GET: Tickets
        public ActionResult Index()
        {
            return View(_context.Tickets.ToList());
        }
        public ActionResult Create()
        {
            ViewBag.employeeNumber = new SelectList(_context.Employees.ToList().Where(x => x.position.Equals("Technician")), "employeeNumber", "fullname");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ticketid,assetnumber,assetowner,subject,priority,description,accomplishstatus,acknowledgestatus,ticketstatus,datecreated,datedue,employeeNumber")] Ticket ticket)
        {
            ticket.accomplishstatus = false;
            ticket.acknowledgestatus = false;
            ticket.ticketstatus = true;
            ticket.datecreated = DateTime.Now;

            if (ModelState.IsValid)
            {
                var asset = _context.Assets.FirstOrDefault(e => e.assetNumber.Equals(ticket.assetnumber));
                if (asset != null)
                {
                    ticket.assetid = asset.assetID;
                    _context.Tickets.Add(ticket);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }
            ViewBag.employeeNumber = new SelectList(_context.Employees.ToList().Where(x => x.position.Equals("Technician")), "employeeNumber", "fullname", ticket.employeeNumber);
            return View(ticket);
        }
        
        public JsonResult GetEmployees(string term)
        {
            List<string> employees;
            employees = _context.Employees.Where(x => x.employeeNumber.StartsWith(term) && x.position != "Technician")
                .Select(n => n.employeeNumber).ToList();
            return Json(employees, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var ticket = _context.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }
        public ActionResult Acknowledge(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = _context.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        [HttpPost, ActionName("Acknowledge")]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm_Acknowledge(int? id)
        {
            Ticket ticket = _context.Tickets.Find(id);

            ticket.acknowledgestatus = true;
             _context.SaveChanges();
             TempData["Success"] = "You acknowledged this ticket";
            return View("Details", new { id = ticket.ticketid });
        }

        public ActionResult Accomplished(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = _context.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }
        [HttpPost, ActionName("Accomplished")]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm_Accomplishment(int? id)
        {
            Ticket ticket = _context.Tickets.Find(id);

            ticket.accomplishstatus = true;
            ticket.ticketstatus = false;
            _context.SaveChanges();
            TempData["Success"] = "Ticket has been completed";
            return View("Details", new { id = ticket.ticketid });
        }
        public JsonResult GetAssets(string term)
        {
            List<string> assets;
            assets = _context.Assets.Where(x => x.assetNumber.StartsWith(term) && x.assignstatus != 0)
                .Select(a => a.assetNumber).ToList();
            return Json(assets, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditProblem(int? id)
        {
            Ticket t = _context.Tickets.Find(id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (t == null)
            {
                return HttpNotFound();
            }

            return View(t);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProblem([Bind(Include = "ticketid,assetnumber,assetid,assetowner,subject,priority,description,accomplishstatus,acknowledgestatus,ticketstatus,datecreated,datedue,employeeNumber")] Ticket t)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(t).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index", "Tickets");
            }
            return View(t);

        }
        public ActionResult Discard(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = _context.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets1/Delete/5
        [HttpPost, ActionName("Discard")]
        [ValidateAntiForgeryToken]
        public ActionResult DiscardConfirmed(int id)
        {
            Ticket ticket = _context.Tickets.Find(id);
            _context.Tickets.Remove(ticket);
            _context.SaveChanges();
            return RedirectToAction("Index", "Tickets");
        }
        public ActionResult EscalateTicket(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Ticket ticket = _context.Tickets.Find(id);
            ViewBag.employeeNumber = new SelectList(_context.Employees.ToList().Where(x => x.position.Equals("Technician")), "employeeNumber", "fullname");
            ViewData["employee"] = _context.Employees.Find(ticket.assetowner).fullname;
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EscalateTicket([Bind(Include = "ticketid,assetnumber,assetowner,subject,priority,description,accomplishstatus,acknowledgestatus,ticketstatus,datedue,employeeNumber")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(ticket).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { id = ticket.ticketid });
            }
            var thisTicket = _context.Tickets.Find(ticket.ticketid);
            ViewBag.employeeNumber = new SelectList(_context.Employees.ToList().Where(x => x.position.Equals("Technician")), "employeeNumber", "fullname", ticket.employeeNumber);
            return View(ticket);
        }

    }
}