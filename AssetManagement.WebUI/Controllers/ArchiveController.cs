using AssetManagement.Domain.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssetManagement.WebUI.Controllers
{
    [Authorize(Roles = "Asset-Manager")]
    public class ArchiveController : Controller
    {
        private readonly AssetManagementEntities context = new AssetManagementEntities();
        //
        // GET: /Archive/
        public ActionResult Index()
        {
            var owners = context.Ownerships;
            return View(owners.ToList());
        }
        public ViewResult Disposed()
        {
            var archive = context.Archives;
            return View(archive.ToList());
        }
	}
}