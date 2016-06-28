using AssetManagement.Business;
using AssetManagement.Domain.Context;
using AssetManagement.Domain.Entities;
using AssetManagement.Domain.QuickResolver;
using AssetManagement.WebUI.ViewModel.Asset;
using AssetManagement.WebUI.ViewModel.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace AssetManagement.WebUI.Controllers
{
    [Authorize(Roles = "Technician")]
    public class TechnicianController : Controller
    {
        private readonly AssetManagementEntities _context = new AssetManagementEntities();
        private AssetResolver list = new AssetResolver();
        AssetLogic al = new AssetLogic();
        //
        // GET: /Technician/
        public ActionResult Tickets(int? page)
        {
            var tickets = _context.Tickets.Where(x => x.employeeNumber.Equals(User.Identity.Name)
                && x.solution == null && x.ticketstatus == true).ToList();

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(tickets.ToPagedList(pageSize,pageNumber));
        }
        public ActionResult Solutions(int? page)
        {
            var tickets = _context.Tickets.Where(t => t.employeeNumber.Equals(User.Identity.Name)
                && t.solution != null && t.ticketstatus == false)
                .ToList();

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(tickets.ToPagedList(pageSize, pageNumber));
        }
        public ActionResult Base(int? page)
        {
            var tickets = _context.Tickets.Where(x => x.solution != null && x.ticketstatus == false).ToList()
                .OrderByDescending(x => x.datecreated);

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(tickets.ToPagedList(pageSize, pageNumber));
        }
        public ActionResult Employees(int? page)
        {
            var query = (from depart in _context.Departments.ToList()
                         join emps in _context.Employees.ToList()
                             on depart.departmentID equals emps.departmentID
                         select new EmployeeViewModel
                         {
                             employeeNumber = emps.employeeNumber,
                             fullname = emps.firstName + " " + emps.lastName,
                             emailAddress = emps.emailAddress,
                             officeNumber = emps.officeNumber,
                             telephoneNumber = emps.telephoneNumber,
                             departmentName = depart.departmentName,
                             position = emps.position
                         })
                        .ToList()
                        .OrderBy(x => x.hireDate);

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(query.ToPagedList(pageSize, pageNumber));
        }
        public ActionResult TicketInfo(int? id)
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
        public ActionResult Assets(int? page)
        {
            var assets = (from a in _context.Assets.ToList()
                          join e in _context.Employees.ToList()
                          on a.employeeNumber equals e.employeeNumber
                          select new AssetListViewModel
                          {
                              assetNumber = a.assetNumber,
                              serialNumber = a.serialNumber,
                              catergory = a.catergory,
                              dateadded = a.dateadded,
                              warranty = a.warranty,
                              assetstatus = a.assignstatus,
                              owner = e.firstName + " " + e.lastName,
                              assigneddate = Convert.ToDateTime(a.assigndate).ToShortDateString(),
                              assetID = a.assetID
                          })
                          .ToList()
                          .Where(x => x.assetstatus == 1);
            int count = assets.ToList().Count;

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(assets.ToPagedList(pageSize,pageNumber));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assets(string search)
        {
            if (search != "")
            {

                var asset = (from a in list.Assets()
                             join e in list.Employees()
                          on a.employeeNumber equals e.employeeNumber
                             select new AssetListViewModel
                             {
                                 assetNumber = a.assetNumber,
                                 serialNumber = a.serialNumber,
                                 manufacturer = a.manufacturer,
                                 catergory = a.catergory,
                                 dateadded = a.dateadded,
                                 warranty = a.warranty,
                                 assetstatus = a.assignstatus,
                                 owner = e.firstName + " " + e.lastName,
                                 assetID = a.assetID,
                                 assigneddate = Convert.ToDateTime(a.assigndate).ToShortDateString()
                             })
                         .Where(x => x.assetNumber.Contains(search.ToUpper()) && x.assetstatus == 1)
                         .ToList();
                return View(asset);
            }
            return View();
        }
        public ActionResult Complete(int? id)
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
        [HttpPost, ActionName("Complete")]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm_Complete(int? id, string solution)
        {
            Ticket ticket = _context.Tickets.Find(id);

            ticket.datecompleted = DateTime.Now;
            ticket.accomplishstatus = true;
            ticket.ticketstatus = false;
            ticket.solution = solution;
            _context.SaveChanges();
            TempData["Success"] = "Ticket has been completed";
            return RedirectToAction("Tickets");
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
            return RedirectToAction("TicketDetails", new { id = ticket.ticketid });
        }
        public ActionResult Report(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var asset = _context.Assets.Single(a => a.assetID == id);

            if (asset == null)
            {
                return HttpNotFound();
            }
            var result = (from a in list.Assets()
                          join e in list.Employees()
                          on a.employeeNumber equals e.employeeNumber
                          select new AssetReport
                          {
                              serialNumber = a.serialNumber,
                              assetNumber = a.assetNumber,
                              catergory = a.catergory,
                              warranty = a.warranty,
                              manufacturer = a.manufacturer,
                              dateadded = a.dateadded,
                              depreciationcost = (al.depreciationCost(a.dateadded, a.costprice)).ToString("R0.00"),
                              assetstatus = a.assignstatus,
                              costprice = (a.costprice).ToString("R0.00"),
                              owner = e.fullname,
                              assetID = a.assetID,
                              assigneddate = a.assigndate,
                              sellprice = (a.costprice - al.depreciationCost(a.dateadded, a.costprice)).ToString("R0.00")
                          }).SingleOrDefault(c => c.assetID == id && c.assetstatus == 1);
            return View(result);
        }
        public ActionResult TicketDetails(int? id)
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
        public ActionResult MyAssets(int? page)
        {
            var assets = (from a in _context.Assets.ToList()
                          join e in _context.Employees.ToList()
                          on a.employeeNumber equals e.employeeNumber
                          select new AssetListViewModel
                          {
                              assetNumber = a.assetNumber,
                              serialNumber = a.serialNumber,
                              catergory = a.catergory,
                              dateadded = a.dateadded,
                              warranty = a.warranty,
                              costprice = a.costprice.ToString("R0.00"),
                              manufacturer = a.manufacturer,
                              assetstatus = a.assignstatus,
                              depreciationcost = (al.depreciationCost(a.dateadded, a.costprice)).ToString("R0.00"),
                              owner = e.fullname,
                              employeenumber = e.employeeNumber,
                              assigneddate = Convert.ToDateTime(a.assigndate).ToShortDateString(),
                              assetID = a.assetID,
                              sell = (a.costprice - al.depreciationCost(a.dateadded, a.costprice)).ToString("R0.00")
                          })
                          .Where(x => x.assetstatus == 1
                              && x.employeenumber.Equals(User.Identity.Name)).ToList();

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(assets.ToPagedList(pageSize,pageNumber));
        }
	}

}