using AssetManagement.Business.HelpDeskSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rotativa;
using System.Web.Mvc;

namespace AssetManagement.WebUI.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            HelpDeskLogic hdl = new HelpDeskLogic();
            return View(hdl);
        }
        public ActionResult ExportPDF()
        {
            return View();
        }
    }
}