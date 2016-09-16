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
    public class MouseController : Controller
    {
        public MouseController()
            : this(new MouseRepository())
        {

        }
        private readonly IMouseRepository _repository;
        private readonly AssetManagementEntities db = new AssetManagementEntities();

        public MouseController(IMouseRepository _repository)
        {
            this._repository = _repository;
        }
        //
        // GET: /Mouse/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            List<Stock> slist = new List<Stock>(db.Stocks.ToList().Where(x => x.category == "Mouse"));
            List<SelectListItem> li = new List<SelectListItem>();
            foreach (var item in slist)
            {
                var man = li.Find(x => x.Value == item.manufacturer);
                if (man == null)
                {
                    li.Add(new SelectListItem { Text = item.manufacturer, Value = item.manufacturer });
                }

            }
            ViewBag.MouseM = li;
            
            return View();
        }

        public JsonResult GetModel(string ddlmanu)
        {
            return Json(db.Stocks.Where(x => x.category == "Mouse" && x.manufacturer == ddlmanu), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(MouseViewModel viewmodel)
        {
            List<Stock> slist = new List<Stock>(db.Stocks.ToList().Where(x => x.category == "Mouse"));
            List<SelectListItem> li = new List<SelectListItem>();
            foreach (var item in slist)
            {
                var man = li.Find(x => x.Value == item.manufacturer);
                if (man == null)
                {
                    li.Add(new SelectListItem { Text = item.manufacturer, Value = item.manufacturer });
                }

            }
            ViewBag.MouseM = li;

            if (ModelState.IsValid)
            {
                try
                {
                    var stock = db.Stocks.FirstOrDefault(m => m.model.Equals(viewmodel.modelName)
                        && m.manufacturer.Equals(viewmodel.manufacturer)
                        && m.category.Equals("Mouse"));

                    if (stock != null && stock.quantity != 0)
                    {
                        var asset = new Asset
                        {
                            manufacturer = viewmodel.manufacturer,
                            serialNumber = viewmodel.serialNumber,
                            dateadded = viewmodel.dateAdded,
                            warranty = viewmodel.warranty + " Months",
                            costprice = viewmodel.costprice,
                            InvoiceNumber = viewmodel.InvoiceNumber
                        };
                        var mouse = new Mouse
                        {
                            serialNumber = viewmodel.serialNumber,
                            manufacturer = viewmodel.manufacturer,
                            modelName = viewmodel.modelName,
                            warranty = viewmodel.warranty + " Months",
                            dateAdded = viewmodel.dateAdded,
                            InvoiceNumber = viewmodel.InvoiceNumber
                        };
                        stock.quantity = stock.quantity - 1;
                        _repository.Insert(asset, mouse);
                        _repository.Save();
                        db.SaveChanges();
                        TempData["Success"] = "Asset has been added!";
                        ModelState.Clear();
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
            
            return View(viewmodel);
        }
	}
}