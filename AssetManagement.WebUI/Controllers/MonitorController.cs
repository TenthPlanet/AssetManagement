using AssetManagement.Business;
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
    [Authorize(Roles = "Asset-Manager")]
    public class MonitorController : Controller
    {
        public MonitorController()
            : this(new MonitorRepository())
        {

        }
        private readonly IMonitorRepository _repo;
        private readonly AssetManagementEntities context = new AssetManagementEntities();
        Stock st = new Stock();

        public MonitorController(IMonitorRepository _repo)
        {
            this._repo = _repo;
        }
        //
        // GET: /Monitor/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            ViewBag.LM = context.Stocks.ToList().Where(x => x.category == "Monitor");
            List<Stock> slist = new List<Stock>(context.Stocks.ToList().Where(x => x.category == "Monitor"));
            List<SelectListItem> li = new List<SelectListItem>();
            foreach (var item in slist)
            {
                var man = li.Find(x => x.Value == item.manufacturer);
                if (man == null)
                {
                    li.Add(new SelectListItem { Text = item.manufacturer, Value = item.manufacturer });
                }

            }
            ViewBag.MM = li;
            return View();
        }

        public JsonResult GetModel(string ddlmanu)
        {
            return Json(context.Stocks.Where(x => x.category == "Monitor" && x.manufacturer == ddlmanu), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(MonitorViewModel viewmodel)
        {
            ViewBag.LM = context.Stocks.ToList().Where(x => x.category == "Monitor");
            List<Stock> slist = new List<Stock>(context.Stocks.ToList().Where(x => x.category == "Monitor"));
            List<SelectListItem> li = new List<SelectListItem>();
            foreach (var item in slist)
            {
                var man = li.Find(x => x.Value == item.manufacturer);
                if (man == null)
                {
                    li.Add(new SelectListItem { Text = item.manufacturer, Value = item.manufacturer });
                }

            }
            ViewBag.MM = li;
            if (ModelState.IsValid)
            {
                try
                {
                    Stock stock = context.Stocks.FirstOrDefault(m => m.model.Equals(viewmodel.modelName)
                        && m.manufacturer.Equals(viewmodel.manufacturer)
                        && m.category.Equals("Monitor"));

                    if (stock != null && stock.quantity != 0)
                    {
                        AssetLogic al = new AssetLogic();
                        var asset = new Asset
                        {
                            manufacturer = viewmodel.manufacturer,
                            serialNumber = viewmodel.serialNumber,
                            dateadded = viewmodel.dateAdded,
                            warranty = viewmodel.warranty + " Months",
                            costprice = viewmodel.costprice,
                            InvoiceNumber = viewmodel.InvoiceNumber,
                            depreciationcost=al.depreciationCost(viewmodel.dateAdded,viewmodel.costprice)
                        };
                        var monitor = new Monitor
                        {
                            serialNumber = viewmodel.serialNumber,
                            manufacturer = viewmodel.manufacturer,
                            modelName = viewmodel.modelName,
                            warranty = viewmodel.warranty + " Months",
                            dateAdded = viewmodel.dateAdded,
                            displaySize = viewmodel.displaySize,
                            InvoiceNumber = viewmodel.InvoiceNumber
                        };
                        stock.quantity = stock.quantity - 1;
                        _repo.Insert(asset, monitor);
                        _repo.Save();
                        context.SaveChanges();
                        TempData["Success"] = "Asset has been added!";
                    }
                    else
                    {
                        ViewBag.Message = "Asset model not available in stock. Update your stock.";
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Message = "Asset not added. Error: " + e.Message;
                }
            }
            ModelState.Clear(); 
            return View(viewmodel);
        }
	}
}