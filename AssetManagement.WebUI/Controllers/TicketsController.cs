using AssetManagement.Domain.Abstract;
using AssetManagement.Domain.Concrete;
using AssetManagement.Domain.Context;
using AssetManagement.Domain.Entities;
using AssetManagement.WebUI.ViewModel;
using AssetManagement.Business.HelpDeskSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using AssetManagement.Business.AssetManagement;

namespace AssetManagement.WebUI.Controllers
{
    [Authorize(Roles = "Administrator,Technician")]
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
        public ActionResult KnowlageBase()
        {
            return View();
        }
        // GET: Tickets
        public ActionResult Index()
        {

            ViewBag.MySolutions = _context.Tickets.Where(t => t.employeeNumber.Equals(User.Identity.Name)
                && t.solution != null && t.ticketstatus == false)
                .ToList().Count();
            ViewBag.KnowledgeBase = _context.Tickets.Where(x => x.solution != null && x.ticketstatus == false).ToList().Count();
            HelpDeskLogic hdl = new HelpDeskLogic();
            TicketReportPerParticipant tr = hdl.getAllTickets();
            TicketReportPerParticipant AdminTickets = hdl.GetParticipantReport(User.Identity.Name);
            List<TicketReportPerParticipant> TicketsFilter = new List<TicketReportPerParticipant>();
            TicketsFilter.Add(tr);
            TicketsFilter.Add(AdminTickets);
            ViewBag.OverDueTickets = hdl.GetOverDueTickets().Count.ToString();
            ViewBag.ControllerName = "HelpDesk";
            ViewData["Role"] = User.IsInRole("Administrator");
            return View(TicketsFilter);
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

            return View(Ticket);
        }
       
        public ActionResult TicketsIndex(int? page)
        {
            var ticket = _context.Tickets.Where(m => m.datecompleted == null && m.solution == null).ToList();
            int PageSize = 4;
            int PageNumber = (page ?? 1);
            return View(ticket.ToPagedList(PageNumber, PageSize));
        }
        public ActionResult Create(string Assets)
        {

            string employeenumber = null, subject = null, body = null, assetnumber = null;

            ViewBag.employeeNumber = new SelectList(_context.Employees.ToList().Where(x => x.position.Equals("Technician") || x.position.Equals("Administrator")), "employeeNumber", "fullname");
            Session["Asset"] = Assets;
            employeenumber = Session["empNo"].ToString();
            subject = Session["Subject"].ToString();
            body = Session["Body"].ToString();
            var catergory = _context.Assets;
            if (Assets == null)
            {
                assetnumber = Session["AssetNumber"].ToString();
            }


            //THIS FROM INBOX MESSAGE

            if (body != " " && subject != " ")
            {
                Ticket tt = new Ticket
                {
                    assetowner = employeenumber,
                    subject = subject,
                    description = body,
                    assetnumber = assetnumber,
                    category = catergory.Single(p => p.assetNumber == assetnumber).catergory
                };
                return View(tt);
            }

            //CREATE TICKET FROM SELECT TYPE VIEW
            //Finding the employee number and catergory
            Asset aa = _context.Assets.ToList().Find(x => x.employeeNumber == employeenumber && x.catergory == Assets);

            if (employeenumber != null && aa != null)
            {
                Ticket t = new Ticket
                {
                    category = Assets,
                    assetnumber = aa.assetNumber,
                    assetowner = aa.employeeNumber,
                };
                return View(t);
            }
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
                return RedirectToAction("Details", new { id = ticket.ticketid });
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
        public ActionResult Acknowladge(int? id)
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
        public ActionResult TicketsControl()
        {
            return View();
        }
        [Authorize(Roles ="Technician")]
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
        [Authorize(Roles ="Technician,Administrator")]
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
        [Authorize(Roles = "Technician,Administrator")]
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MySolutions(string search)
        {
            if (search != null)
            {
                var tickets = _context.Tickets.Where(t => t.employeeNumber.Equals(User.Identity.Name)
                    && t.solution != null && t.ticketstatus == false)
                    .ToList()
                    .Where(x => x.subject.Contains(search) || x.description.Contains(search));
                return View(tickets);
            }
            else
            {
                return RedirectToAction("Index");
            }
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
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Solutions(string search)
        {
            if (search != null)
            {
                var tickets = _context.Tickets.Where(x => x.solution != null && x.ticketstatus == false).ToList()
                    .Where(x => x.solution.Contains(search) || x.subject.Contains(search) || x.description.Contains(search))
                    .OrderByDescending(x => x.datecreated);
                return View(tickets);
            }
            return RedirectToAction("Index");
        }
        public ActionResult SolutionPortal(string search)
        {
            if (search != null)
            {
                var tickets = _context.Tickets.Where(x => x.solution != null && x.ticketstatus == false).ToList()
                    .Where(x => x.solution.Contains(search) || x.subject.Contains(search) || x.description.Contains(search))
                    .OrderByDescending(x => x.datecreated);
                return View(tickets);
            }
            return RedirectToAction("SolutionPortal");
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
        //Helpdesk Inbox
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inbox(string search)
        {
            if (search != "")
            {
                var query = (from a in _context.Contactus.ToList()
                             select new ContactUsViewModel
                             {
                                 id = a.contactId,
                                 read = a.read,
                                 subject = a.subject,
                                 body = a.body,
                                 username = a.userName,
                                 datesent = a.datesent
                             })
                                 //.OrderBy(x => x.datesent)
                                 .Where(x => x.body.Contains(search) || x.subject.Contains(search));
                int count = (query.ToList().Where(x => x.read.Equals(false))).Count();
                int count2 = query.ToList().Count();
                ViewBag.Mail = count2;
                ViewBag.Inbox = count;
                return View(query);
            }
            else
            {
                return RedirectToAction("AllMessages");
            }

            //var inbox = _context.Contactus.ToList();
            //return View(inbox);
        }
        public ActionResult AllMessages()
        {
            ViewBag.AllMessages = _context.Contactus.ToList().Count();
            ViewBag.OpenedMessages = _context.Contactus.ToList().Where(x => x.read.Equals(true)).Count();
            ViewBag.ClosedMessages = _context.Contactus.ToList().Where(x => x.read.Equals(false)).Count();
            return View();
        }

        public ActionResult Inbox()
        {
            var query = (from a in _context.Contactus.ToList()
                         select new ContactUsViewModel
                         {
                             id = a.contactId,
                             read = a.read,
                             subject = a.subject,
                             body = a.body,
                             username = a.userName,
                             datesent = a.datesent
                         })
                         .OrderByDescending(x => x.datesent);
            //int PageSize = 6;
            //int PageNumber = (page ?? 1);
            return View(query);
            //var inbox = _context.Contactus.ToList();
            //return View(inbox);
        }

        public ActionResult InboxCount()
        {
            ViewBag.InboxNote = (_context.Contactus.ToList().Where(x => x.read.Equals(false))).Count();
            return View();
        }

        public ActionResult InboxDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var inbox = _context.Contactus.Find(id);

            Session["empNo"] = inbox.userName.ToString();
            Session["Subject"] = inbox.subject.ToString();
            Session["Body"] = inbox.body.ToString();
            if (inbox.category != null)
            {
                Session["AssetNumber"] = inbox.category.ToString();
            }
            var screenshots = _context.Screenshots.Where(m => m.contactId == inbox.contactId).ToList();
            if (inbox == null)
            {
                return HttpNotFound();
            }

            var data = new Tuple<ContactUs, IEnumerable<Screenshot>>(inbox, screenshots);
            inbox.read = true;
            _context.SaveChanges();
            return View(data);
        }
        public ActionResult unReadmail(int? page)
        {
            var query = _context.Contactus.Where(x => x.read.Equals(false)).ToList();
            int PageSize = 4;
            int PageNumber = (page ?? 1);
            return View(query.ToPagedList(PageNumber, PageSize));
        }
        public ActionResult OpenedMail(int? page)
        {
            var query = _context.Contactus.Where(x => x.read.Equals(true)).ToList();
            int PageSize = 4;
            int PageNumber = (page ?? 1);
            return View(query.ToPagedList(PageNumber, PageSize));
        }

        public ActionResult Report()
        {

            return View();
        }

        public ActionResult OverDueTickets()
        {
            var hdl = new HelpDeskLogic();
            return View(hdl.GetOverDueTickets());
        }

        public ActionResult General()
        {
            ViewBag.employeeNumber = new SelectList(_context.Employees.ToList().Where(x => x.position.Equals("Technician") || x.position.Equals("Administrator")), "employeeNumber", "fullname");

            string eno = null, sub = null, bodd = null;

            eno = Session["empNo"].ToString();
            sub = Session["Subject"].ToString();
            bodd = Session["Body"].ToString();

            GeneralTicketViewModel gtt;
            gtt = new GeneralTicketViewModel
            {
                employee = eno,
                subject = sub,
                description = bodd
            };
            return View(gtt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult General([Bind(Include = "ticketid,assetnumber,assetowner,subject,priority,description,accomplishstatus,acknowledgestatus,ticketstatus,datecreated,datedue,employeeNumber")] GeneralTicketViewModel model)
        {
            string eno = Session["empNo"].ToString();
            model.accomplishstatus = false;
            model.acknowledgestatus = false;
            model.ticketstatus = true;
            model.datecreated = DateTime.Now;

            if (ModelState.IsValid)
            {
                Ticket tt = new Ticket
                {

                    accomplishstatus = model.accomplishstatus,
                    acknowledgestatus = model.acknowledgestatus,
                    ticketstatus = model.ticketstatus,
                    datecreated = model.datecreated,
                    employeeNumber = model.employeeNumber,
                    subject = model.subject,
                    description = model.description,
                    datedue = model.datedue,
                    assetnumber = null,
                    assetowner = eno,

                };
                _context.Tickets.Add(tt);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.employeeNumber = new SelectList(_context.Employees.ToList().Where(x => x.position.Equals("Technician") || x.position.Equals("Administrator")), "employeeNumber", "fullname", model.employeeNumber);

            return View(model);
        }
        public ActionResult TicketsCount()//Total to notify tickets
        {
            int tickets = _context.Tickets.Where(m => m.datecompleted == null && m.solution == null).ToList().Count();
            int overdueCount = _context.Tickets.ToList().FindAll(x => x.datedue < System.DateTime.Now).ToList().Count();
            ViewBag.Tickets = tickets + overdueCount;
            return View();
        }
        public ActionResult OverDue()
        {
            int overdueCount = _context.Tickets.ToList().FindAll(x => x.datedue < System.DateTime.Now).ToList().Count();
            ViewBag.Overdue = overdueCount;
            return View();
        }
        public ActionResult DueTickets()//unsolved tickets
        {
            int tickets = _context.Tickets.Where(m => m.datecompleted == null && m.solution == null).ToList().Count();
            ViewBag.Tickets = tickets;
            return View();
        }
        public PartialViewResult _OpenedTickets()
        {
            HelpDeskLogic hdl = new HelpDeskLogic();
            var id = ViewData["id"].ToString();
            var tickets = hdl.OpenTickets(id);
            return PartialView("_OpenedTickets",tickets);
        }
        public PartialViewResult _UnAknowlaged(string id)
        {
            HelpDeskLogic hdl = new HelpDeskLogic();
            var tickets = hdl.UnAknowlagedTickets(id);
            return PartialView("_UnAknowlaged",tickets);
        }
        public PartialViewResult _Completed(string id)
        {
            HelpDeskLogic hdl = new HelpDeskLogic();
            var tickets = hdl.CompletedTickets(id);
            return PartialView("_Completed",tickets);
        }
        public PartialViewResult _AllTickets(string id)
        {
            HelpDeskLogic hdl = new HelpDeskLogic();
            var tickets = hdl.AllTickets(id);
            return PartialView("_AllTickets",tickets);
        }
        public ActionResult TicketsFilter(string id)
        {
            HelpDeskLogic hdl = new HelpDeskLogic();
            Session["Technician"] = hdl.GetEmployee(id).fullname;
            ViewData["id"] = hdl.GetEmployee(id).employeeNumber;
            return View(hdl);
        }
        public PartialViewResult _TicketsPerParticipant(string id)
        {
            HelpDeskLogic hdl = new HelpDeskLogic();
            return PartialView("_TicketsPerParticipant",hdl.GetParticipantReport(id));
        }
        [HttpGet]
        public ActionResult AcknowlageTicket(string id)
        {
            HelpDeskLogic hdl = new HelpDeskLogic();
            var ticket = hdl.GetTicket(int.Parse(id));
            return View(ticket);
        }
        [HttpPost]
        [ActionName("AcknowlageTicket")]
        public ActionResult Acknowlage_Ticket(string id)
        {
            Ticket ticket = _context.Tickets.Find(int.Parse(id));
            ticket.acknowledgestatus = true;
            TryUpdateModel(ticket);
            _context.SaveChanges();
            TempData["Success"] = "You have acknowledged this ticket";
            return RedirectToAction("Ticket", "Tickets", new { id = ticket.ticketid });
        }

        [HttpPost]
        [ActionName("CloseTicket")]
        public ActionResult Ticket(string id, string solution)
        {
            Ticket ticket = _context.Tickets.Find(id);
            ticket.datecompleted = DateTime.Now;
            ticket.accomplishstatus = true;
            ticket.ticketstatus = false;
            ticket.solution = solution;
            _context.SaveChanges();
            TempData["Success"] = "Ticket has been completed";
            return View();
        }
    }
}