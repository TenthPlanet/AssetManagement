using AssetManagement.Business.HelpDeskSystem;
using AssetManagement.Domain.Context;
using AssetManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssetManagement.WebUI.Controllers
{
    public class ReplacementPartController : Controller
    {
        [HttpPost]
        public ActionResult ReplacementPart(ReplacementPart part)
        {
            part.associatedAsset = Session["AssetNumber"] as string;
            part.associatedTicket = Convert.ToInt16(Session["TicketID"]);
            HelpDeskLogic hdl = new HelpDeskLogic();
            hdl.AddReplacementPart(part);
            return RedirectToAction("Ticket","Technician",new { id = part.associatedTicket});
        }

        [HttpGet]
        public ActionResult ReplacementPart()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ReplacementParts()
        {
            HelpDeskLogic hdl = new HelpDeskLogic();

            return View(hdl.spareParts);
        }
    }
}