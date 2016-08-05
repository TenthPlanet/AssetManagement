using AssetManagement.Domain.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace AssetManagement.WebUI.Controllers
{
    [Authorize(Roles = "Asset-Manager")]
    public class ArchiveController : Controller
    {
        private readonly AssetManagementEntities context = new AssetManagementEntities();
        //
        // GET: /Archive/
        public ActionResult Index(int?page)
        {

            var owners = context.Ownerships;
            int PageSize = 6;
            int PageNumber = (page ?? 1);
            return View(owners.ToList().ToPagedList(PageNumber,PageSize));
        }
        public ViewResult Disposed(int? page)
        {
            var archive = context.Archives;
            int PageSize = 6;
            int PageNumber = (page ?? 1);
            return View(archive.ToList().ToPagedList(PageNumber, PageSize));
            
        }
	}
}