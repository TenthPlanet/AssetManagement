using AssetManagement.Domain.Abstract;
using AssetManagement.Domain.Concrete;
using AssetManagement.Domain.Context;
using AssetManagement.Domain.Entities;
using AssetManagement.WebUI.Models;
using AssetManagement.WebUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AssetManagement.WebUI.Controllers
{
    [Authorize(Roles = "Administrator")]
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
            var ticket = _context.Tickets.Where(m => m.datecompleted == null && m.solution == null).ToList();
            return View(ticket);
        }
        public ActionResult Create()
        {
            ViewBag.employeeNumber = new SelectList(_context.Employees.ToList().Where(x => x.position.Equals("Technician") || x.position.Equals("Administrator")), "employeeNumber", "fullname");
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
            ViewBag.employeeNumber = new SelectList(_context.Employees.ToList().Where(x => x.position.Equals("Technician") || x.position.Equals("Administrator")), "employeeNumber", "fullname", ticket.employeeNumber);
            return View(ticket);
        }

        //Still incomplete
        public ActionResult Escalate(int? id)
        {
            ViewBag.employeeNumber = new SelectList(_context.Employees.ToList().Where(x => x.position.Equals("Technician") || x.position.Equals("Administrator")), "employeeNumber", "fullname");

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Escalate(int? id, string employeeNumber, string subject, DateTime datedue, string priority)
        {
            var ticket = _context.Tickets.Find(id);
            if (ModelState.IsValid)
            {
                ticket.subject = subject;
                ticket.datedue = datedue;
                ticket.priority = priority;
                ticket.employeeNumber = employeeNumber;
                _context.SaveChanges();
                return RedirectToAction("Details", new { id = ticket.ticketid });
            }
            
            ViewBag.employeeNumber = new SelectList(_context.Employees.ToList().Where(x => x.position.Equals("Technician") || x.position.Equals("Administrator")), "employeeNumber", "fullname", ticket.employeeNumber);
            return View(ticket);

        }
        public ActionResult EditProblem(int? id)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProblem(int? id, string subject, string description, string priority, DateTime datedue)
        {
            var ticket = _context.Tickets.Single(c => c.ticketid == id);
            if (ModelState.IsValid)
            {
                ticket.subject = subject;
                ticket.description = description;
                ticket.priority = priority;
                ticket.datedue = datedue;
                _context.SaveChanges();
                return RedirectToAction("Details", new { id =ticket.ticketid });
            }
            return View(ticket);
        }
        public ActionResult Discard(int? id)
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

        [HttpPost, ActionName("Discard")]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm_Discard(int? id)
        {
            var ticket = _context.Tickets.Find(id);
            _context.Tickets.Remove(ticket);
            _context.SaveChanges();
            return RedirectToAction("Index");
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
            TempData["Success"] = "You have acknowledged this ticket";
            return RedirectToAction("Details", new { id = ticket.ticketid });
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
        public ActionResult Confirm_Accomplishment(int? id, string solution)
        {
            Ticket ticket = _context.Tickets.Find(id);
            
            ticket.datecompleted = DateTime.Now;
            ticket.accomplishstatus = true;
            ticket.ticketstatus = false;
            ticket.solution = solution;
            _context.SaveChanges();
            TempData["Success"] = "Ticket has been completed";
            return RedirectToAction("Details", new { id = ticket.ticketid });
        }
        public JsonResult GetAssets(string term)
        {
            List<string> assets;
            assets = _context.Assets.Where(x => x.assetNumber.StartsWith(term) && x.assignstatus != 0)
                .Select(a => a.assetNumber).ToList();
            return Json(assets, JsonRequestBehavior.AllowGet);
        }





        //Tickets assigned to the help desk
        public ActionResult MyTickets()
        {
            var tickets = _context.Tickets.Where(x => x.employeeNumber.Equals(User.Identity.Name)
                && x.solution == null && x.ticketstatus == true).ToList();
            return View(tickets);
        }

        public ActionResult TechnicianTickets()
        {
            var tickets = _context.Tickets.Where(x => x.employeeNumber.Equals(User.Identity.Name) 
                && x.solution == null && x.ticketstatus == true).ToList();
            return View(tickets);
        }
        //Solutions of a specific the helpdesk admin
        public ActionResult MySolutions()
        {
            var tickets = _context.Tickets.Where(t => t.employeeNumber.Equals(User.Identity.Name)
                && t.solution != null && t.ticketstatus == false)
                .ToList();
            return View(tickets);
        }
        //Knowledge base, all the tickets that have been a completed 
        //All tickets that have a solution
        [AllowAnonymous]
        public ActionResult Solutions()
        {
            var tickets = _context.Tickets.Where(x => x.solution != null && x.ticketstatus == false).ToList()
                .OrderByDescending(x => x.datecreated);
            return View(tickets);
        }
        //Full solution details
        [AllowAnonymous]
        public ActionResult Info(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = _context.Tickets.FirstOrDefault(a => a.ticketid == id 
                && a.solution != null
                && a.ticketstatus == false);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        
    }
}