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
    public class PCController : Controller
    {
        public PCController()
            : this(new PCRepository())
        {

        }
        private readonly IPCRepository _repo;
        private readonly AssetManagementEntities db = new AssetManagementEntities();

        public PCController(IPCRepository _repo)
        {
            this._repo = _repo;
        }
        //
        // GET: /PC/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            ViewBag.PCM = db.Stocks.ToList().Where(x => x.category == "PCBox");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(PCViewModel viewmodel)
        {
            ViewBag.PCM = db.Stocks.ToList().Where(x=>x.category=="PCBox");
            if (ModelState.IsValid)
            {
                try
                {
                    var stock = db.Stocks.FirstOrDefault(m => m.model.Equals(viewmodel.modelName)
                        && m.manufacturer.Equals(viewmodel.manufacturer)
                        && m.category.Equals("PCBox"));


                    if (stock != null && stock.quantity != 0)
                    {
                        var asset = new Asset
                        {
                            manufacturer = viewmodel.manufacturer,
                            serialNumber = viewmodel.serialNumber,
                            dateadded = viewmodel.dateAdded,
                            warranty = viewmodel.warranty + " Months",
                            costprice = viewmodel.costprice
                        };
                        var pc = new PCBox
                        {
                            serialNumber = viewmodel.serialNumber,
                            manufacturer = viewmodel.manufacturer,
                            modelName = viewmodel.modelName,
                            warranty = viewmodel.warranty + " Months",
                            dateAdded = viewmodel.dateAdded,
                            HDD = viewmodel.HDD,
                            OS = viewmodel.OS,
                            RAM = viewmodel.RAM,
                        };
                        stock.quantity = stock.quantity - 1;
                        _repo.Insert(asset, pc);
                        _repo.Save();
                        db.SaveChanges();
                        TempData["Success"] = "Asset has been added!";
                    }
                    else
                    {
                        ViewBag.Message = "Asset not available in stock. Update your stock.";
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