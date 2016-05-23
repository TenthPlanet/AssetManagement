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
    public class PrinterController : Controller
    {
        public PrinterController() : this(new PrinterRepository()) 
        { }

        private readonly IPrinterRepository repository;
        private readonly AssetManagementEntities context = new AssetManagementEntities();

        public PrinterController(IPrinterRepository repository)
        {
            this.repository = repository;
        }
        //
        // GET: /Printer/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            ViewBag.PrM = context.Stocks.ToList().Where(x => x.category == "Printer");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(PrinterViewModel viewmodel)
        {
            ViewBag.PrM = context.Stocks.ToList().Where(x => x.category == "Printer");
            if (ModelState.IsValid)
            {
                try
                {
                    var stock = context.Stocks.FirstOrDefault(m => m.model.Equals(viewmodel.modelName)
                        && m.manufacturer.Equals(viewmodel.manufacturer)
                        && m.category.Equals("Printer"));

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
                        var printer = new Printer
                        {
                            serialNumber = viewmodel.serialNumber,
                            manufacturer = viewmodel.manufacturer,
                            modelName = viewmodel.modelName,
                            warranty = viewmodel.warranty + " Months",
                            dateAdded = viewmodel.dateAdded,
                        };
                        stock.quantity = stock.quantity - 1;
                        repository.Insert(asset, printer);
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