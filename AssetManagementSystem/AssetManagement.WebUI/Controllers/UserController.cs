using AssetManagement.Business;
using AssetManagement.Domain.Context;
using AssetManagement.Domain.Entities;
using AssetManagement.WebUI.ViewModel;
using AssetManagement.WebUI.ViewModel.Asset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AssetManagement.WebUI.Controllers
{
    [Authorize(Roles = "General Staff")]
    public class UserController : Controller
    {
        private readonly AssetManagementEntities _context = new AssetManagementEntities();
        AssetLogic al = new AssetLogic();
        //
        // GET: /User/
        //public ActionResult Index()
        //{
        //    var tickets = _context.Tickets.Where(x => x.solution != null && x.ticketstatus == false).ToList()
        //         .OrderByDescending(x => x.datecreated);
        //    return View(tickets);
        //}

        //Knowledge base list
        public ActionResult Base()
        {
            var tickets = _context.Tickets.Where(x => x.solution != null && x.ticketstatus == false).ToList()
                 .OrderByDescending(x => x.datecreated);
            return View(tickets);
        }

        //All asset owned by an employee
        public ActionResult Assets()
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

            return View(assets);
        }

        //Detailed problem and solution
        public ActionResult Solution(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ticket = _context.Tickets.FirstOrDefault(a => a.ticketid == id
                && a.solution != null
                && a.ticketstatus == false);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        //Contacting the helpdesk
        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactUs(ContactUsViewModel model)
        {
            if (ModelState.IsValid && Request.IsAuthenticated)
            {
                var contact = new ContactUs
                {
                    subject = model.subject,
                    body = model.body,
                    userName = User.Identity.Name,
                    read = false,
                    datesent = DateTime.Now
                };
                _context.Contactus.Add(contact);
                TempData["Success"] = "Message was sent. ";
                _context.SaveChanges();
            }
            ModelState.Clear();
            return View();
        }
    }
}