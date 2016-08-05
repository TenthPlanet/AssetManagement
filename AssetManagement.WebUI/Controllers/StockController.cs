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
using PagedList.Mvc;
using PagedList;

namespace AssetManagement.WebUI.Controllers
{
    [Authorize(Roles = "Asset-Manager")]
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
        [AllowAnonymous]
        public ActionResult Index(int?page)
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
            int PageSize = 6;
            int PageNumber = (page ?? 1);
            return View(query.ToPagedList(PageNumber, PageSize));
         
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
                        && x.manufacturer.Equals(viewmodel.Manaufacturer) && x.category.Equals(viewmodel.Catergory));

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