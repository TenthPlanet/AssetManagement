using AssetManagement.Business.HelpDeskSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rotativa;
using System.Web.Mvc;

namespace AssetManagement.WebUI.Controllers
{
    [AllowAnonymous]
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            HelpDeskLogic hdl = new HelpDeskLogic();
            return View(hdl);
        }
        
        public ActionResult FinencialReport()
        {
            var hdl = new HelpDeskLogic();
            var rpt = new Business.HelpDeskSystem.FinencialReport()
            {
                invoices = hdl.invoices,
                replacementParts = hdl.spareParts,
                TotalAssetCost = hdl.TotalAssetsCost(),
                TotalSparePartsCost = hdl.TotalPartsCost(),
                assets = hdl.assets,
                TotalCost = hdl.TotalCost(),
                qty = hdl.Qunatity()
            };
            ViewBag.count = rpt.qty + rpt.assets.Count;
            return View(rpt);
        }
        public ActionResult ExportPDF()
        {
            return View();
        }
    }
} 