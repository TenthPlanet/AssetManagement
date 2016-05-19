using AssetManagement.Domain.Abstract;
using AssetManagement.Domain.Concrete;
using AssetManagement.Domain.Context;
using AssetManagement.Domain.Entities;
using AssetManagement.WebUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var ticket = (from emps in _context.Employees
                          join tics in _context.Tickets
                          on emps.employeeNumber equals tics.assetowner
                          select new TicketViewModel
                          {
                              accomplishstatus = tics.accomplishstatus,
                              acknowledgestatus = tics.acknowledgestatus,
                              assetid = tics.assetid,
                              assetnumber = tics.assetnumber,
                              fullname = emps.fullname,
                              datecreated = tics.datecreated,
                              datedue = tics.datedue,
                              assetowner = tics.assetowner,
                              subject = tics.subject,
                              ticketid = tics.ticketid,
                              ticketstatus = tics.ticketstatus,
                              priority = tics.priority,
                              description = tics.description
                          }).FirstOrDefault(x => x.ticketid == id);

            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Acknowledge(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var ticket = _context.Tickets.FirstOrDefault(a => a.ticketid == id);

            if (ticket != null)
            {
                ticket.acknowledgestatus = true;
                _context.SaveChanges();
            }
            else
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Accomplished(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var ticket = _context.Tickets.FirstOrDefault(a => a.ticketid == id);

            if (ticket != null)
            {
                ticket.accomplishstatus = true;
                _context.SaveChanges();
            }
            else
            {
                return HttpNotFound();
            }
            return View(ticket);
        }
        public JsonResult GetAssets(string term)
        {
            List<string> assets;
            assets = _context.Assets.Where(x => x.assetNumber.StartsWith(term) && x.assignstatus != 0)
                .Select(a => a.assetNumber).ToList();
            return Json(assets, JsonRequestBehavior.AllowGet);
        }
    }
}