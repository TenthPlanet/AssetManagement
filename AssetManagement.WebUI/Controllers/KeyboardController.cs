﻿using AssetManagement.Business;
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
    [Authorize]
    public class KeyboardController : Controller
    {
        public KeyboardController()
            : this(new KeyboardRepository())
        {

        }
        private readonly IKeyboardRepository _repository;
        private readonly AssetManagementEntities context = new AssetManagementEntities();
        Keyboard key = new Keyboard();

        public KeyboardController(IKeyboardRepository _repository)
        {
            this._repository = _repository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            List<Stock> slist = new List<Stock>(context.Stocks.ToList().Where(x => x.category == "Keyboard"));
            List<SelectListItem> li = new List<SelectListItem>();
            foreach (var item in slist)
            {
                var man = li.Find(x => x.Value == item.manufacturer);
                if (man == null)
                {
                    li.Add(new SelectListItem { Text = item.manufacturer, Value = item.manufacturer });
                }

            }
            ViewBag.KeyBM = li;

            
            return View();
        }

        public JsonResult GetModel(string ddlmanu)
        {
            return Json(context.Stocks.Where(x => x.category == "Keyboard" && x.manufacturer == ddlmanu), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(KeyboardViewModel viewmodel)
        {
            List<Stock> slist = new List<Stock>(context.Stocks.ToList().Where(x => x.category == "Keyboard"));
            List<SelectListItem> li = new List<SelectListItem>();
            foreach (var item in slist)
            {
                var man = li.Find(x => x.Value == item.manufacturer);
                if (man == null)
                {
                    li.Add(new SelectListItem { Text = item.manufacturer, Value = item.manufacturer });
                }

            }
            ViewBag.KeyBM = li;

            if (ModelState.IsValid)
            {
                try
                {
                    Stock stock = context.Stocks.FirstOrDefault(m => m.model.Equals(viewmodel.modelName)
                        && m.manufacturer.Equals(viewmodel.manufacturer)
                        && m.category.Equals("Keyboard"));

                    if (stock != null && stock.quantity != 0)
                    {
                        AssetLogic al = new AssetLogic();
                        var asset = new Asset
                        {
                            manufacturer = viewmodel.manufacturer,
                            serialNumber = viewmodel.serialNumber,
                            dateadded = viewmodel.dateAdded,
                            warranty = viewmodel.warranty+" Months",
                            costprice = viewmodel.costprice,
                            InvoiceNumber = viewmodel.InvoiceNumber,
                            depreciationcost=al.depreciationCost(viewmodel.dateAdded,viewmodel.costprice)

                        };
                        var keyboard = new Keyboard
                        {
                            serialNumber = viewmodel.serialNumber,
                            manufacturer = viewmodel.manufacturer,
                            modelName = viewmodel.modelName,
                            warranty = viewmodel.warranty + " Months",
                            dateAdded = viewmodel.dateAdded,
                            InvoiceNumber = viewmodel.InvoiceNumber
                        };
                        stock.quantity = stock.quantity - 1;
                        _repository.Insert(asset, keyboard);
                        _repository.Save();
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

            DateTime endofwarranty = key.dateAdded.AddDays(1).AddMonths(Convert.ToInt32(viewmodel.warranty)).AddDays(-1);
            ViewBag.KeyEnd = endofwarranty;

            ModelState.Clear();
            return View(viewmodel);
        }
	}
}