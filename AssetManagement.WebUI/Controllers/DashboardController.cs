using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetManagement.WebUI.Controllers;
using AssetManagement.Domain.Context;
using AssetManagement.Domain.QuickResolver;
using AssetManagement.WebUI.ViewModel.Asset;
using AssetManagement.Domain.Concrete;
using AssetManagement.Domain.Entities;

namespace AssetManagement.WebUI.Controllers
{
    public class DashboardController : Controller
    {

        private readonly AssetManagementEntities context = new AssetManagementEntities();
        private AssetResolver list = new AssetResolver();
        private StockRepository stock = new StockRepository();
        Stock stt = new Stock();
         

        //
        // GET: /Dashboard/
        public ActionResult Dashboard()
        {
            int s = 0;
            ViewBag.getAssets = list.Assets().Count;
            ViewBag.getEmployee = list.Employees().Count;
            foreach(Stock st in context.Stocks.ToList())
            {
                s+= st.quantity;
                
            }
            ViewBag.getStock=s;


            return View();
        }

    }
}