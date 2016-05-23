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
            ViewBag.LM = context.Stocks.ToList().Where(x => x.category == "Laptop");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(LaptopViewModel viewmodel)
        {
            ViewBag.LM = context.Stocks.ToList().Where(x=>x.category=="Laptop");
            if (ModelState.IsValid)
            {
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
                            warranty = viewmodel.warranty,
                            costprice = viewmodel.costprice
                        };
                        var laptop = new Laptop
                        {
                            serialNumber = viewmodel.serialNumber,
                            manufacturer = viewmodel.manufacturer,
                            modelName = viewmodel.modelName,
                            warranty = viewmodel.warranty,
                            dateAdded = viewmodel.dateAdded,
                            HDD = viewmodel.HDD,
                            OS = viewmodel.OS,
                            RAM = viewmodel.RAM,
                            screenSize = viewmodel.screenSize,
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