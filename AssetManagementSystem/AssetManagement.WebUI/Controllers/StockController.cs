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
    public class StockController : Controller
    {
        public StockController()
            : this(new StockRepository())
        {

        }
        private readonly IStockRepository repo;
        private readonly AssetManagementEntities context = new AssetManagementEntities();

        public StockController(IStockRepository repo)
        {
            this.repo = repo;
        }

        //
        // GET: /Stock/
        public ActionResult Index()
        {
            var query = (from assets in repo.Stocks()
                         select new StockViewModel 
                         { 
                             Model = assets.model,
                             Catergory = assets.category,
                             Manaufacturer = assets.manufacturer,
                             Quantity = assets.quantity.ToString()
                         }).ToList()
                         .OrderBy(m => m.Quantity);
            return View(query);
        }

        public ViewResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(StockViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    Stock stck = context.Stocks.FirstOrDefault(x => x.model.Equals(viewmodel.Model)
                        && x.manufacturer.Equals(viewmodel.Manaufacturer));

                    if (stck != null)
                    {
                        stck.quantity = stck.quantity + (Convert.ToInt32(viewmodel.Quantity));
                        context.SaveChanges();
                        TempData["Success"] = "Asset stock has been updated!";
                        return View();
                    }
                    else
                    {
                        var stock = new Stock
                        {
                            category = viewmodel.Catergory,
                            model = viewmodel.Model,
                            manufacturer = viewmodel.Manaufacturer,
                            quantity = Convert.ToInt32(viewmodel.Quantity)
                        };
                        repo.Insert(stock);
                        repo.Save();
                    }
                    TempData["Success"] = "New asset stock has been added!";
                }
                catch (Exception e)
                {
                    ViewBag.Message = "Asset stock not added. Error: " + e.Message;
                }
            }
            ModelState.Clear();
            return View();
        }
	}
}