using AssetManagement.Business;
using AssetManagement.Domain.Abstract;
using AssetManagement.Domain.Concrete;
using AssetManagement.Domain.Context;
using AssetManagement.Domain.Entities;
using AssetManagement.WebUI.QuickResolvers;
using AssetManagement.WebUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssetManagement.WebUI.Controllers
{
    [Authorize(Roles = "Asset-Manager")]
    public class LaptopController : Controller
    {
        public LaptopController() : this(new LaptopRepository())
        { }

        private readonly ILaptopRepository repository;
        private readonly AssetManagementEntities context = new AssetManagementEntities();


        public LaptopController(ILaptopRepository repository)
        {
            this.repository = repository;
        }
        //
        // GET: /Laptop/
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Add()
        {
            List<Stock> slist =new List<Stock>(context.Stocks.ToList().Where(x => x.category == "Laptop"));
            List<SelectListItem> li = new List<SelectListItem>();
            foreach (var item in slist)
            {
                var man = li.Find(x => x.Value == item.manufacturer);
                if(man==null)
                {
                    li.Add(new SelectListItem { Text = item.manufacturer, Value = item.manufacturer });
                }
                
            }
            ViewBag.LL = li;
            return View();
        }
        
        public JsonResult GetModel(string ddlmanu)
        {
            return Json(context.Stocks.Where(x => x.category == "Laptop" && x.manufacturer == ddlmanu), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(LaptopViewModel viewmodel, string modelName)
        {
            List<Stock> slist = new List<Stock>(context.Stocks.ToList().Where(x => x.category == "Laptop"));
            List<SelectListItem> li = new List<SelectListItem>();
            foreach (var item in slist)
            {
                var man = li.Find(x => x.Value == item.manufacturer);
                if (man == null)
                {
                    li.Add(new SelectListItem { Text = item.manufacturer, Value = item.manufacturer });
                }

            }
            viewmodel.modelName = modelName;
            ViewBag.LL = li;

            ViewBag.LM = context.Stocks.ToList().Where(x=>x.category=="Laptop");
            if (ModelState.IsValid)
            {
                AssetLogic al = new AssetLogic();
                try
                {
                    Stock stock = context.Stocks.FirstOrDefault(m => m.model.Equals(viewmodel.modelName) 
                        && m.manufacturer.Equals(viewmodel.manufacturer)
                        && m.category.Equals("Laptop"));

                    if (stock != null && stock.quantity != 0)
                    {
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
                        var laptop = new Laptop
                        {
                            serialNumber = viewmodel.serialNumber,
                            manufacturer = viewmodel.manufacturer,
                            modelName = viewmodel.modelName,
                            warranty = viewmodel.warranty + " Months",
                            dateAdded = viewmodel.dateAdded,
                            HDD = viewmodel.HDD,
                            OS = viewmodel.OS,
                            RAM = viewmodel.RAM,
                            screenSize = viewmodel.screenSize,
                            InvoiceNumber = viewmodel.InvoiceNumber,
                        };
                        stock.quantity = stock.quantity - 1;
                        repository.Insert(asset, laptop);
                        repository.Save();
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