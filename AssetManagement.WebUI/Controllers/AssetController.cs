using AssetManagement.Domain.Context;
using AssetManagement.Domain.Entities;
using AssetManagement.Domain.QuickResolver;
using AssetManagement.WebUI.ViewModel.Asset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AssetManagement.Business;
using PagedList.Mvc;
using PagedList;

namespace AssetManagement.WebUI.Controllers
{
    [Authorize]
    public class AssetController : Controller
    {
        private readonly AssetManagementEntities context = new AssetManagementEntities();
        private AssetResolver list = new AssetResolver();
        AssetLogic al = new AssetLogic();

        //
        // GET: /Asset/
        public ActionResult Index(int?page)
        {
            var assets = (from a in list.Assets()
                          select new AssetListViewModel
                          {
                              assetNumber = a.assetNumber,
                              serialNumber = a.serialNumber,
                              manufacturer = a.manufacturer,
                              catergory = a.catergory,
                              dateadded = a.dateadded,
                              warranty = a.warranty,
                              assetstatus = a.assignstatus,
                              assetID = a.assetID
                          })
                          .ToList()
                          .Where(x => x.assetstatus == 0);
            int count = assets.ToList().Count;
            ViewBag.Items = count;

            int PageSize = 6;
            int PageNumber = (page ?? 1);
            return View(assets.ToPagedList(PageNumber, PageSize));
             
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assigned(string search,int?page)
        {
            if (search != "")
            {

                var asset = (from a in list.Assets()
                             join e in list.Employees()
                          on a.employeeNumber equals e.employeeNumber
                             select new AssetListViewModel
                             {
                                 assetNumber = a.assetNumber,
                                 serialNumber = a.serialNumber,
                                 manufacturer = a.manufacturer,
                                 catergory = a.catergory,
                                 dateadded = a.dateadded,
                                 warranty = a.warranty,
                                 assetstatus = a.assignstatus,
                                 owner = e.firstName + " " + e.lastName,
                                 assetID = a.assetID,
                                 assigneddate = Convert.ToDateTime(a.assigndate).ToShortDateString()
                             })
                         .Where(x => x.assetNumber.Contains(search.ToUpper()) && x.assetstatus == 1)
                         .ToList();
                int PageSize = 6;
                int PageNumber = (page ?? 1);
                return View(asset.ToPagedList(PageNumber, PageSize));
               
            }
            return View();
        }

        public ActionResult Assigned(int?page)
        {

            var assets = (from a in list.Assets()
                          join e in list.Employees()
                          on a.employeeNumber equals e.employeeNumber
                          select new AssetListViewModel
                          {
                              assetNumber = a.assetNumber,
                              serialNumber = a.serialNumber,
                              catergory = a.catergory,
                              dateadded = a.dateadded,
                              warranty = a.warranty,
                              assetstatus = a.assignstatus,
                              owner = e.firstName + " " + e.lastName,
                              assigneddate = Convert.ToDateTime(a.assigndate).ToShortDateString(),
                              assetID = a.assetID
                          })
                          .ToList()
                          .Where(x => x.assetstatus == 1);
            int PageSize = 6;
            int PageNumber = (page ?? 1);
            //int count = assets.ToList().Count;
            //ViewBag.Items = count;
            //ViewBag.Category = list.getCategories();
            return View(assets.ToPagedList(PageNumber,PageSize));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Depreciation(string search,int ?page)
        {
            if (search != "")
            {

                var asset = (from a in list.Assets()
                             select new AssetListViewModel
                             {
                                 serialNumber = a.serialNumber,
                                 assetNumber = a.assetNumber,
                                 catergory = a.catergory,
                                 warranty = a.warranty,
                                 manufacturer = a.manufacturer,
                                 dateadded = a.dateadded,
                                 depreciationcost = (al.depreciationCost(a.dateadded, a.costprice)).ToString("R0.00"),
                                 assetstatus = a.assignstatus,
                                 costprice = a.costprice.ToString("R0.00")
                             })
                         .Where(x => x.assetNumber.Contains(search.ToUpper()) && x.assetstatus == 1 && (!((DateTime.Now.Year - x.dateadded.Year) < 1)))
                         .ToList();
                int PageSize = 6;
                int PageNumber = (page ?? 1);
                return View(asset.ToPagedList(PageNumber, PageSize));
            }
            return View();
        }
        public ActionResult Depreciation(int?page)
        {
            var assets = (from a in list.Assets()
                          join e in list.Employees() on a.employeeNumber equals e.employeeNumber
                          select new AssetListViewModel
                          {
                              serialNumber = a.serialNumber,
                              assetNumber = a.assetNumber,
                              catergory = a.catergory,
                              warranty = a.warranty,
                              manufacturer = a.manufacturer,
                              dateadded = a.dateadded,
                              depreciationcost = (al.depreciationCost(a.dateadded, a.costprice)).ToString("R0.00"),
                              assetstatus = a.assignstatus,
                              costprice = (a.costprice).ToString("R0.00")
                          })
                          .ToList()
                          .Where(x => x.assetstatus == 1 && (!((DateTime.Now.Year - x.dateadded.Year) < 1)));
            ViewBag.Category = context.Categories.ToList();
            int PageSize = 6;
            int PageNumber = (page ?? 1);
            return View(assets.ToPagedList(PageNumber, PageSize));
            
        }
        public ActionResult FullyDepreciated(int? page)
        {
            //var ass = context.Assets.ToList().Find(x=>x.assetID==id);
            var assets = (from a in list.Assets()
                          join e in list.Employees() on a.employeeNumber equals e.employeeNumber
                          select new AssetListViewModel
                          {
                              serialNumber = a.serialNumber,
                              assetNumber = a.assetNumber,
                              catergory = a.catergory,
                              warranty = a.warranty,
                              manufacturer = a.manufacturer,
                              dateadded = a.dateadded,
                              depreciationcost = (al.depreciationCost(a.dateadded, a.costprice)).ToString("R0.00"),
                              assetstatus = a.assignstatus,
                              costprice = (a.costprice).ToString("R0.00")
                          })
                          .ToList()
                          .Where(x => x.assetstatus == 1 && (!((DateTime.Now.Year - x.dateadded.Year) < 1)) && x.depreciationcost.Equals("R0.00"));
            ViewBag.Category = context.Categories.ToList();
            int PageSize = 6;
            int PageNumber = (page ?? 1);
            return View(assets.ToPagedList(PageNumber, PageSize));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FullyDepreciated(string search, int? page)
        {
            //var ass = context.Assets.ToList().Find(x => x.assetID == id);
            if (search != "")
            {

                var asset = (from a in list.Assets()
                             select new AssetListViewModel
                             {
                                 serialNumber = a.serialNumber,
                                 assetNumber = a.assetNumber,
                                 catergory = a.catergory,
                                 warranty = a.warranty,
                                 manufacturer = a.manufacturer,
                                 dateadded = a.dateadded,
                                 depreciationcost = (al.depreciationCost(a.dateadded, a.costprice)).ToString("R0.00"),
                                 assetstatus = a.assignstatus,
                                 costprice = a.costprice.ToString("R0.00")
                             })
                         .Where(x => x.assetNumber.Contains(search.ToUpper()) && x.assetstatus == 1 && (!((DateTime.Now.Year - x.dateadded.Year) < 1)) && x.depreciationcost.Equals("R0.00"))
                         .ToList();
                int PageSize = 6;
                int PageNumber = (page ?? 1);
                return View(asset.ToPagedList(PageNumber, PageSize));
            }
            return View();
        }

        public ActionResult FullyDAssets()
        {
            List<Asset> ass = context.Assets.ToList().FindAll(x=>x.depreciationcost==x.costprice&&x.assignstatus==1);
            int fullyCount = context.Assets.ToList().FindAll(x => x.depreciationcost ==x.costprice && x.assignstatus == 1).ToList().Count();
            ViewBag.fullydue = fullyCount;
            return View(ass);

        }
        [HttpPost]
        public ActionResult FullyDAssets(int id)
        {
            Asset asse = context.Assets.ToList().Find(x=>x.assetID==id);

            return View(asse);

        }
        public ActionResult Assign()
        {

            ViewBag.Employee = context.Employees.ToList();
            ViewBag.Printers = context.Printers.ToList().Where(x => x.assignStatus == 0);
            ViewBag.Laptops = context.Laptops.ToList().Where(x => x.assignStatus == 0);
            ViewBag.Monitors = context.Monitors.ToList().Where(x => x.assignStatus == 0);
            ViewBag.PCs = context.PCBoxes.ToList().Where(x => x.assignStatus == 0);
            ViewBag.Keyboards = context.Keyboards.ToList().Where(x => x.assignStatus == 0);
            ViewBag.Mice = context.Mice.ToList().Where(x => x.assignStatus == 0);


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assign(AssignViewModel assign)
        {
            try
            {
                ViewBag.Employee = context.Employees.ToList();
                ViewBag.Printers = context.Printers.ToList().Where(x => x.assignStatus == 0);
                ViewBag.Laptops = context.Laptops.ToList().Where(x => x.assignStatus == 0);
                ViewBag.Monitors = context.Monitors.ToList().Where(x => x.assignStatus == 0);
                ViewBag.PCs = context.PCBoxes.ToList().Where(x => x.assignStatus == 0);
                ViewBag.Keyboards = context.Keyboards.ToList().Where(x => x.assignStatus == 0);
                ViewBag.Mice = context.Mice.ToList().Where(x => x.assignStatus == 0);

                var employee = context.Employees.Find(assign.employeeID);
                var printer = context.Printers.Find(assign.printerID);
                var laptop = context.Laptops.Find(assign.laptopID);
                var monitor = context.Monitors.Find(assign.monitorID);
                var pcs = context.PCBoxes.Find(assign.pcBoxID);
                var keyboard = context.Keyboards.Find(assign.keyboardID);
                var mouse = context.Mice.Find(assign.mouseID);


                if (employee != null)
                {
                    if (printer != null)
                    {
                        Asset assetPrinter = context.Assets.SingleOrDefault(x => x.assetNumber == printer.assetNumber);
                        if (assetPrinter != null)
                        {
                            printer.employeeNumber = employee.employeeNumber;
                            assetPrinter.assignstatus = 1;
                            printer.assignStatus = assetPrinter.assignstatus;
                            assetPrinter.employeeNumber = employee.employeeNumber;
                            assetPrinter.assigndate = DateTime.Now;
                            printer.assigndate = assetPrinter.assigndate;
                        }

                    }
                    if (laptop != null)
                    {
                        Asset assetLaptop = context.Assets.SingleOrDefault(x => x.assetNumber == laptop.assetNumber);
                        if (assetLaptop != null)
                        {
                            laptop.employeeNumber = employee.employeeNumber;
                            assetLaptop.assignstatus = 1;
                            laptop.assignStatus = assetLaptop.assignstatus;
                            assetLaptop.employeeNumber = employee.employeeNumber;
                            assetLaptop.assigndate = DateTime.Now;
                            laptop.assigndate = assetLaptop.assigndate;
                        }
                    }
                    if (monitor != null)
                    {
                        Asset assetMonitor = context.Assets.SingleOrDefault(x => x.assetNumber == monitor.assetNumber);
                        if (assetMonitor != null)
                        {
                            monitor.employeeNumber = employee.employeeNumber;
                            assetMonitor.assignstatus = 1;
                            monitor.assignStatus = assetMonitor.assignstatus;
                            assetMonitor.employeeNumber = employee.employeeNumber;
                            assetMonitor.assigndate = DateTime.Now;
                            monitor.assigndate = assetMonitor.assigndate;
                        }
                    }
                    if (pcs != null)
                    {
                        Asset assetPCs = context.Assets.SingleOrDefault(x => x.assetNumber == pcs.assetNumber);
                        if (assetPCs != null)
                        {
                            pcs.employeeNumber = employee.employeeNumber;
                            assetPCs.assignstatus = 1;
                            pcs.assignStatus = assetPCs.assignstatus;
                            assetPCs.employeeNumber = employee.employeeNumber;
                            assetPCs.assigndate = DateTime.Now;
                            pcs.assigndate = assetPCs.assigndate;
                        }
                    }
                    if (keyboard != null)
                    {
                        Asset assetKeyboard = context.Assets.SingleOrDefault(x => x.assetNumber == keyboard.assetNumber);
                        if (assetKeyboard != null)
                        {
                            keyboard.employeeNumber = employee.employeeNumber;
                            assetKeyboard.assignstatus = 1;
                            keyboard.assignStatus = assetKeyboard.assignstatus;
                            assetKeyboard.employeeNumber = employee.employeeNumber;
                            assetKeyboard.assigndate = DateTime.Now;
                            keyboard.assigndate = assetKeyboard.assigndate;
                        }
                    }
                    if (mouse != null)
                    {
                        Asset assetMouse = context.Assets.SingleOrDefault(x => x.assetNumber == mouse.assetNumber);
                        if (assetMouse != null)
                        {
                            mouse.employeeNumber = employee.employeeNumber;
                            assetMouse.assignstatus = 1;
                            mouse.assignStatus = assetMouse.assignstatus;
                            assetMouse.employeeNumber = employee.employeeNumber;
                            assetMouse.assigndate = DateTime.Now;
                            mouse.assigndate = assetMouse.assigndate;
                        }
                    }
                    if (printer == null && laptop == null && monitor == null && pcs == null && keyboard == null && mouse == null)
                    {
                        ViewBag.Message = "Select item(s) to assign...";
                    }

                }
                else { ViewBag.Message = "Select employee to assign item(s) to..."; }

                context.SaveChanges();

                if (employee != null)
                {
                    if (printer != null || laptop != null || monitor != null || pcs != null || keyboard != null || mouse != null)
                    {
                        TempData["Success"] = "Assignment to " + employee.firstName + " " + employee.lastName + " successful!";
                    }
                }

            }
            catch (Exception e)
            {
                ViewBag.Message = "Asset not assigned. Error: " + e.Message;
            }
            return View(assign);
        }

        public ActionResult DisposeItem(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = context.Assets.Single(x => x.assetID == id);

            if (asset == null)
            {
                return HttpNotFound();
            }
            return View(asset);
        }
    
        [HttpPost, ActionName("DisposeItem")]
        [ValidateAntiForgeryToken]
        public ActionResult DisposalConfirmed(int id)
        {
            var asset = context.Assets.Single(x => x.assetID == id);
            var laptop = context.Laptops.FirstOrDefault(x => x.assetID == id);
            var monitor = context.Monitors.FirstOrDefault(x => x.assetID == id);
            var tower = context.PCBoxes.FirstOrDefault(x => x.assetID == id);
            var keyboard = context.Keyboards.FirstOrDefault(x => x.assetID == id);
            var mouse = context.Mice.FirstOrDefault(x => x.assetID == id);
            var printer = context.Printers.FirstOrDefault(x => x.assetID == id);



            if (laptop != null && asset != null)
            {
                var archive = new Archive
                {
                    assetNumber = laptop.assetNumber,
                    category = laptop.catergory,
                    dateAdded = laptop.dateAdded,
                    dateDisposed = DateTime.Now,
                    employeeName = laptop.Employee.fullname,
                    employeeNumber = laptop.employeeNumber
                };
                context.Archives.Add(archive);
                context.Laptops.Remove(laptop);
            }
            if (monitor != null && asset != null)
            {
                var archive = new Archive
                {
                    assetNumber = monitor.assetNumber,
                    category = monitor.catergory,
                    dateAdded = monitor.dateAdded,
                    dateDisposed = DateTime.Now,
                    employeeName = monitor.Employee.fullname,
                    employeeNumber = monitor.employeeNumber
                };
                context.Archives.Add(archive);
                context.Monitors.Remove(monitor);
            }
            if (tower != null && asset != null)
            {
                var archive = new Archive
                {
                    assetNumber = tower.assetNumber,
                    category = tower.catergory,
                    dateAdded = tower.dateAdded,
                    dateDisposed = DateTime.Now,
                    employeeName = tower.Employee.fullname,
                    employeeNumber = tower.employeeNumber
                };
                context.Archives.Add(archive);
                context.PCBoxes.Remove(tower);
            }
            if (keyboard != null && asset != null)
            {
                var archive = new Archive
                {
                    assetNumber = keyboard.assetNumber,
                    category = keyboard.catergory,
                    dateAdded = keyboard.dateAdded,
                    dateDisposed = DateTime.Now,
                    employeeName = keyboard.Employee.fullname,
                    employeeNumber = keyboard.employeeNumber
                };
                context.Archives.Add(archive);
                context.Keyboards.Remove(keyboard);
            }
            if (mouse != null && asset != null)
            {
                var archive = new Archive
                {
                    assetNumber = mouse.assetNumber,
                    category = mouse.catergory,
                    dateAdded = mouse.dateAdded,
                    dateDisposed = DateTime.Now,
                    employeeName = mouse.Employee.fullname,
                    employeeNumber = mouse.employeeNumber
                };
                context.Archives.Add(archive);
                context.Mice.Remove(mouse);
            }
            if (printer != null && asset != null)
            {
                var archive = new Archive
                {
                    assetNumber = printer.assetNumber,
                    category = printer.catergory,
                    dateAdded = printer.dateAdded,
                    dateDisposed = DateTime.Now,
                    employeeName = printer.Employee.fullname,
                    employeeNumber = printer.employeeNumber
                };
                context.Archives.Add(archive);
                context.Printers.Remove(printer);
            }

            context.Assets.Remove(asset);
            context.SaveChanges();
            return RedirectToAction("Assigned");
        }

        public ActionResult Return(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = context.Assets.Single(x => x.assetID == id);

            if (asset == null)
            {
                return HttpNotFound();
            }
            return View(asset);
        }

        [HttpPost, ActionName("Return")]
        [ValidateAntiForgeryToken]
        public ActionResult ReturnConfirmed(int id)
        {
            var asset = context.Assets.Single(x => x.assetID == id);
            var laptop = context.Laptops.FirstOrDefault(x => x.assetID == id);
            var monitor = context.Monitors.FirstOrDefault(x => x.assetID == id);
            var tower = context.PCBoxes.FirstOrDefault(x => x.assetID == id);
            var keyboard = context.Keyboards.FirstOrDefault(x => x.assetID == id);
            var mouse = context.Mice.FirstOrDefault(x => x.assetID == id);
            var printer = context.Printers.FirstOrDefault(x => x.assetID == id);

            if (asset != null)
            {
                if (laptop != null)
                {
                    var stock = context.Stocks.FirstOrDefault(f => f.model.Equals(laptop.modelName)
                    && f.manufacturer.Equals(laptop.manufacturer)
                    && f.manufacturer.Equals(asset.manufacturer));

                    if (stock != null)
                    {
                        stock.quantity = stock.quantity + 1;
                        context.Laptops.Remove(laptop);
                    }
                }

                if (monitor != null)
                {
                    var stock = context.Stocks.FirstOrDefault(f => f.model.Equals(monitor.modelName)
                    && f.manufacturer.Equals(monitor.manufacturer)
                    && f.manufacturer.Equals(asset.manufacturer));

                    if (stock != null)
                    {
                        stock.quantity = stock.quantity + 1;
                        context.Monitors.Remove(monitor);
                    }
                }

                if (tower != null)
                {
                    var stock = context.Stocks.FirstOrDefault(f => f.model.Equals(tower.modelName)
                    && f.manufacturer.Equals(tower.manufacturer)
                    && f.manufacturer.Equals(asset.manufacturer));

                    if (stock != null)
                    {
                        stock.quantity = stock.quantity + 1;
                        context.PCBoxes.Remove(tower);
                    }
                }
                if (keyboard != null)
                {
                    var stock = context.Stocks.FirstOrDefault(f => f.model.Equals(keyboard.modelName)
                    && f.manufacturer.Equals(keyboard.manufacturer)
                    && f.manufacturer.Equals(asset.manufacturer));

                    if (stock != null)
                    {
                        stock.quantity = stock.quantity + 1;
                        context.Keyboards.Remove(keyboard);
                    }
                }
                if (mouse != null)
                {
                    var stock = context.Stocks.FirstOrDefault(f => f.model.Equals(mouse.modelName)
                    && f.manufacturer.Equals(mouse.manufacturer)
                    && f.manufacturer.Equals(asset.manufacturer));

                    if (stock != null)
                    {
                        stock.quantity = stock.quantity + 1;
                        context.Mice.Remove(mouse);
                    }
                }

                if (printer != null)
                {
                    var stock = context.Stocks.FirstOrDefault(f => f.model.Equals(printer.modelName)
                    && f.manufacturer.Equals(printer.manufacturer)
                    && f.manufacturer.Equals(asset.manufacturer));

                    if (stock != null)
                    {
                        stock.quantity = stock.quantity + 1;
                        context.Printers.Remove(printer);
                    }
                }
                context.Assets.Remove(asset);
                context.SaveChanges();

                TempData["Success"] = "Item returned to Stock.";
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult TempDev()
        {
            return View(context.TemporalDevices.ToList());
        }
        [HttpPost]
        public ActionResult TempDev(int id)
        {
            return View(context.TemporalDevices.ToList());
        }

        public ActionResult FullyDepreciatedAssets()
        {
            List<Asset> fullyassets = context.Assets.ToList().FindAll(x => x.depreciationcost == x.costprice);
            int fullyCount = context.Assets.ToList().FindAll(x => x.depreciationcost ==x.costprice && x.assignstatus==1).ToList().Count();
            ViewBag.fullydue = fullyCount;
            return View(fullyassets);
        }

        public ActionResult DeallocateTempDev(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var asset = context.TemporalDevices.Single(a => a.TempID == id);

            if (asset == null)
            {
                return HttpNotFound();
            }
            return View(asset);
        }
        [HttpPost]
        public ActionResult DeallocateTempDev(int id)
        {
            var asset = context.TemporalDevices.Single(a => a.TempID == id);
            if(asset!=null)
            {
                asset.employeeFullname = "";
                asset.employeeNumber = "";
            }
            context.SaveChanges();
            return RedirectToAction("TempDev");
        }
        public ActionResult TempAssignAsset(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var asset = context.TemporalDevices.Single(a => a.TempID == id);

            if (asset == null)
            {
                return HttpNotFound();
            }
            return View();
        }
        [HttpPost]
        public ActionResult TempAssignAsset(int? id,string employeenumber)
        {
            var Employee = context.Employees.SingleOrDefault(emp => emp.employeeNumber.Equals(employeenumber));
            var asset = context.TemporalDevices.Single(a => a.TempID == id);

            var printer = context.TemporalDevices.Find(asset.category=="");
            var laptop = context.TemporalDevices.Find(asset.category == "");
            var monitor = context.TemporalDevices.Find(asset.category == "");
            var pcs = context.TemporalDevices.Find(asset.category == "");
            var keyboard = context.TemporalDevices.Find(asset.category == "");
            var mouse = context.TemporalDevices.Find(asset.category == "");


            return View();
        }
        public ActionResult AssignTempDev(int id)
        {
            var temp = context.TemporalDevices.ToList().Find(x => x.TempID == id);
            return View(temp);
        }
        [HttpPost]
        public ActionResult AssignTempDev(int?id, string employeenumber)
        {
            var Employee = context.Employees.SingleOrDefault(emp => emp.employeeNumber.Equals(employeenumber));
            var temp = context.TemporalDevices.ToList().Find(x=>x.TempID==id);
            if(Employee!=null)
            {
                temp.employeeNumber = Employee.employeeNumber;
                temp.employeeFullname = Employee.fullname;
            }
            context.SaveChanges();
            return RedirectToAction("TempDev");
        }
        public ActionResult DisposeTempDev(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemporalDevice td = context.TemporalDevices.ToList().Find(x => x.TempID == id);
            if (td == null)
            {
                return HttpNotFound();
            }
            
            return View(td);
        }
        [HttpPost]
        public ActionResult DisposeTempDev(int? id)
        {
            var tempDev = context.TemporalDevices.FirstOrDefault(x => x.TempID == id);


            var archive = new Archive
            {
                assetNumber = tempDev.serialNumber,
                category = tempDev.category,
                dateAdded = tempDev.dateadded,
                dateDisposed = DateTime.Now,
                employeeName = "N/A",
                employeeNumber = "N/A"
            };

            context.Archives.Add(archive);
            context.TemporalDevices.Remove(tempDev);
            context.SaveChanges();
            return RedirectToAction("TempDev");
        }

        public ActionResult Deallocate(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var asset = context.Assets.Single(a => a.assetID == id);

            if (asset == null)
            {
                return HttpNotFound();
            }
            return View(asset);
        }

        [HttpPost, ActionName("Deallocate")]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm_Dealocation(int id)
        {
            var asset = context.Assets.Single(x => x.assetID == id && x.assignstatus == 1);
            var employee = context.Employees.FirstOrDefault(em => em.employeeNumber.Equals(asset.employeeNumber));

            var laptop = context.Laptops.FirstOrDefault(x => x.assetID == id && x.assignStatus == 1);
            var monitor = context.Monitors.FirstOrDefault(x => x.assetID == id && x.assignStatus == 1);
            var printer = context.Printers.FirstOrDefault(x => x.assetID == id && x.assignStatus == 1);
            var mouse = context.Mice.FirstOrDefault(x => x.assetID == id && x.assignStatus == 1);
            var keyboard = context.Keyboards.FirstOrDefault(x => x.assetID == id && x.assignStatus == 1);
            var tower = context.PCBoxes.FirstOrDefault(x => x.assetID == id && x.assignStatus == 1);


            AssetLogic al = new AssetLogic();

            if (asset != null && employee != null)
            {

                if (laptop != null)
                {
                    
                    var ownership = new OwnershipHistory
                    {
                        assetNumber = laptop.assetNumber,
                        category = laptop.catergory,
                        assignDate = Convert.ToDateTime(laptop.assigndate),
                        delocateDate = DateTime.Now,
                        employeeNumber = employee.employeeNumber,
                        employeeFullname = employee.fullname
                    };
                    asset.assignstatus = 0;
                    asset.assigndate = null;
                    asset.employeeNumber = null;
                    laptop.assignStatus = 0;
                    laptop.assigndate = null;
                    laptop.employeeNumber = null;
                    

                    if (al.depreciationCost(asset.dateadded, asset.costprice) < 1)
                    {
                        var tempo = new TemporalDevice
                        {
                            assetID = laptop.assetID,
                            serialNumber=laptop.serialNumber,
                            model = laptop.modelName,
                            manufacturer = laptop.manufacturer,
                            category = laptop.catergory,
                            dateadded = laptop.dateAdded
                        };
     
                        context.TemporalDevices.Add(tempo);
                    }
                    context.Ownerships.Add(ownership);
                    context.Assets.Remove(asset);
                    context.Laptops.Remove(laptop);
                    

                }

                if (monitor != null)
                {
                    var ownership = new OwnershipHistory
                    {
                        assetNumber = monitor.assetNumber,
                        category = monitor.catergory,
                        assignDate = Convert.ToDateTime(monitor.assigndate),
                        delocateDate = DateTime.Now,
                        employeeNumber = employee.employeeNumber,
                        employeeFullname = employee.fullname
                    };
                    asset.assignstatus = 0;
                    asset.assigndate = null;
                    asset.employeeNumber = null;
                    monitor.assignStatus = 0;
                    monitor.assigndate = null;
                    monitor.employeeNumber = null;

                    if (al.depreciationCost(asset.dateadded, asset.costprice) == 0)
                    {
                        var tempo = new TemporalDevice
                        {
                            assetID = monitor.assetID,
                            serialNumber = monitor.serialNumber,
                            model = monitor.modelName,
                            manufacturer = monitor.manufacturer,
                            category = monitor.catergory,
                            dateadded = monitor.dateAdded
                        };

                        context.TemporalDevices.Add(tempo);
                    }
                    context.Ownerships.Add(ownership);
                    context.Assets.Remove(asset);
                    context.Monitors.Remove(monitor);

                }

                if (printer != null)
                {
                    
                    var ownership = new OwnershipHistory
                    {
                        assetNumber = printer.assetNumber,
                        category = printer.catergory,
                        assignDate = Convert.ToDateTime(printer.assigndate),
                        delocateDate = DateTime.Now,
                        employeeNumber = employee.employeeNumber,
                        employeeFullname = employee.fullname
                    };
                    asset.assignstatus = 0;
                    asset.assigndate = null;
                    asset.employeeNumber = null;
                    printer.assignStatus = 0;
                    printer.assigndate = null;
                    printer.employeeNumber = null;

                    if (al.depreciationCost(asset.dateadded, asset.costprice) < 1)
                    {
                        var tempo = new TemporalDevice
                        {
                            assetID = printer.assetID,
                            serialNumber = printer.serialNumber,
                            model = printer.modelName,
                            manufacturer = printer.manufacturer,
                            category = printer.catergory,
                            dateadded = printer.dateAdded
                        };

                        context.TemporalDevices.Add(tempo);
                    }
                    context.Ownerships.Add(ownership);
                    context.Assets.Remove(asset);
                    context.Printers.Remove(printer);
                }

                if (mouse != null)
                {
                    var ownership = new OwnershipHistory
                    {
                        assetNumber = mouse.assetNumber,
                        category = mouse.catergory,
                        assignDate = Convert.ToDateTime(mouse.assigndate),
                        delocateDate = DateTime.Now,
                        employeeNumber = employee.employeeNumber,
                        employeeFullname = employee.fullname
                    };
                    asset.assignstatus = 0;
                    asset.assigndate = null;
                    asset.employeeNumber = null;
                    mouse.assignStatus = 0;
                    mouse.assigndate = null;
                    mouse.employeeNumber = null;

                    if (al.depreciationCost(asset.dateadded, asset.costprice) == 0)
                    {
                        var tempo = new TemporalDevice
                        {
                            assetID = mouse.assetID,
                            serialNumber = mouse.serialNumber,
                            model = mouse.modelName,
                            manufacturer = mouse.manufacturer,
                            category = mouse.catergory,
                            dateadded = mouse.dateAdded
                        }
                        ;
                        context.TemporalDevices.Add(tempo);
                    }
                    context.Ownerships.Add(ownership);
                    context.Assets.Remove(asset);
                    context.Mice.Remove(mouse);
                }

                if (keyboard != null)
                {
                    
                    var ownership = new OwnershipHistory
                    {
                        assetNumber = keyboard.assetNumber,
                        category = keyboard.catergory,
                        assignDate = Convert.ToDateTime(keyboard.assigndate),
                        delocateDate = DateTime.Now,
                        employeeNumber = employee.employeeNumber,
                        employeeFullname = employee.fullname
                    };
                    asset.assignstatus = 0;
                    asset.assigndate = null;
                    asset.employeeNumber = null;
                    keyboard.assignStatus = 0;
                    keyboard.assigndate = null;
                    keyboard.employeeNumber = null;

                    if (al.depreciationCost(asset.dateadded, asset.costprice) == 0)
                    {
                        var tempo = new TemporalDevice
                        {
                            assetID = keyboard.assetID,
                            serialNumber = keyboard.serialNumber,
                            model = keyboard.modelName,
                            manufacturer = keyboard.manufacturer,
                            category = keyboard.catergory,
                            dateadded = keyboard.dateAdded
                        }
                        ;

                        context.TemporalDevices.Add(tempo);
                    }
                    context.Ownerships.Add(ownership);
                    context.Assets.Remove(asset);
                    context.Keyboards.Remove(keyboard);
                }

                if (tower != null)
                {
                    var ownership = new OwnershipHistory
                    {
                        assetNumber = tower.assetNumber,
                        category = tower.catergory,
                        assignDate = Convert.ToDateTime(tower.assigndate),
                        delocateDate = DateTime.Now,
                        employeeNumber = employee.employeeNumber,
                        employeeFullname = employee.fullname
                    };
                    asset.assignstatus = 0;
                    asset.assigndate = null;
                    asset.employeeNumber = null;
                    tower.assignStatus = 0;
                    tower.assigndate = null;
                    tower.employeeNumber = null;

                    if (al.depreciationCost(asset.dateadded, asset.costprice) == 0)
                    {
                        var tempo = new TemporalDevice
                        {
                            assetID = tower.assetID,
                            serialNumber = tower.serialNumber,
                            model = tower.modelName,
                            manufacturer = tower.manufacturer,
                            category = tower.catergory,
                            dateadded = tower.dateAdded
                        }
                        ;
                        context.TemporalDevices.Add(tempo);
                    }
                    context.Ownerships.Add(ownership);
                    context.Assets.Remove(asset);
                    context.PCBoxes.Remove(tower);
                }
            }
            context.SaveChanges();
            TempData["Success"] = "Asset dealocation successful...";
            return RedirectToAction("Assigned");
        }
        public ActionResult AssignAsset(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var asset = context.Assets.Single(a => a.assetID == id);

            if (asset == null)
            {
                return HttpNotFound();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignAsset(int? id, string employeenumber)
        {
            var Employee = context.Employees.SingleOrDefault(emp => emp.employeeNumber.Equals(employeenumber));
            var asset = context.Assets.SingleOrDefault(a => a.assetID == id);

            var printer = context.Printers.Find(asset.assetNumber);
            var laptop = context.Laptops.Find(asset.assetNumber);
            var monitor = context.Monitors.Find(asset.assetNumber);
            var pcs = context.PCBoxes.Find(asset.assetNumber);
            var keyboard = context.Keyboards.Find(asset.assetNumber);
            var mouse = context.Mice.Find(asset.assetNumber);

            if (Employee != null)
            {
                if (printer != null)
                {
                    printer.employeeNumber = Employee.employeeNumber;
                    asset.assignstatus = 1;
                    printer.assignStatus = asset.assignstatus;
                    asset.employeeNumber = Employee.employeeNumber;
                    asset.assigndate = DateTime.Now;
                    printer.assigndate = asset.assigndate;
                }

                if (laptop != null)
                {
                    laptop.employeeNumber = Employee.employeeNumber;
                    asset.assignstatus = 1;
                    laptop.assignStatus = asset.assignstatus;
                    asset.employeeNumber = Employee.employeeNumber;
                    asset.assigndate = DateTime.Now;
                    laptop.assigndate = asset.assigndate;
                }
                if (monitor != null)
                {
                    monitor.employeeNumber = Employee.employeeNumber;
                    asset.assignstatus = 1;
                    monitor.assignStatus = asset.assignstatus;
                    asset.employeeNumber = Employee.employeeNumber;
                    asset.assigndate = DateTime.Now;
                    monitor.assigndate = asset.assigndate;

                }
                if (pcs != null)
                {
                    pcs.employeeNumber = Employee.employeeNumber;
                    asset.assignstatus = 1;
                    pcs.assignStatus = asset.assignstatus;
                    asset.employeeNumber = Employee.employeeNumber;
                    asset.assigndate = DateTime.Now;
                    pcs.assigndate = asset.assigndate;

                }
                if (keyboard != null)
                {

                    keyboard.employeeNumber = Employee.employeeNumber;
                    asset.assignstatus = 1;
                    keyboard.assignStatus = asset.assignstatus;
                    asset.employeeNumber = Employee.employeeNumber;
                    asset.assigndate = DateTime.Now;
                    keyboard.assigndate = asset.assigndate;
                }
                if (mouse != null)
                {

                    mouse.employeeNumber = Employee.employeeNumber;
                    asset.assignstatus = 1;
                    mouse.assignStatus = asset.assignstatus;
                    asset.employeeNumber = Employee.employeeNumber;
                    asset.assigndate = DateTime.Now;
                    mouse.assigndate = asset.assigndate;
                }
                if (employeenumber == null)
                {
                    ViewBag.Message = "Enter employee's full name...";
                }
                context.SaveChanges();
                TempData["Success"] = "Asset successfully assigned...";
                return RedirectToAction("Index");

            }
            return View();

        }
        public JsonResult GetEmployees(string term)
        {
            List<string> employees;
            employees = context.Employees.Where(x => x.employeeNumber.StartsWith(term))
                .Select(n => n.employeeNumber).ToList();
            return Json(employees, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Report(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var asset = context.Assets.Single(a => a.assetID == id);

            if (asset == null)
            {
                return HttpNotFound();
            }
            var result = (from a in list.Assets()
                          join e in list.Employees()
                          on a.employeeNumber equals e.employeeNumber
                          select new AssetReport
                          {
                              serialNumber = a.serialNumber,
                              assetNumber = a.assetNumber,
                              catergory = a.catergory,
                              warranty = a.warranty,
                              manufacturer = a.manufacturer,
                              dateadded = a.dateadded,
                              depreciationcost = (al.depreciationCost(a.dateadded, a.costprice)).ToString("R0.00"),
                              assetstatus = a.assignstatus,
                              costprice = (a.costprice).ToString("R0.00"),
                              owner = e.fullname,
                              assetID = a.assetID,
                              assigneddate = a.assigndate,
                              sellprice = (a.costprice - al.depreciationCost(a.dateadded, a.costprice)).ToString("R0.00")
                          }).SingleOrDefault(c => c.assetID == id && c.assetstatus == 1);
            return View(result);
        }

        //The technician must be able to see the list of assets
        //and their respective owners
        [Authorize(Roles = "Technician")]
        public ActionResult AllAssets(int?page)
        {
            var assets = (from a in list.Assets()
                          join e in list.Employees()
                          on a.employeeNumber equals e.employeeNumber
                          select new AssetListViewModel
                          {
                              assetNumber = a.assetNumber,
                              serialNumber = a.serialNumber,
                              catergory = a.catergory,
                              dateadded = a.dateadded,
                              warranty = a.warranty,
                              assetstatus = a.assignstatus,
                              owner = e.firstName + " " + e.lastName,
                              assigneddate = Convert.ToDateTime(a.assigndate).ToShortDateString(),
                              assetID = a.assetID
                          })
                          .ToList()
                          .Where(x => x.assetstatus == 1);
            int count = assets.ToList().Count;
            int PageSize = 6;
            int PageNumber = (page ?? 1);
            return View(assets.ToPagedList(PageNumber,PageSize));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AllAssets(string search)
        {
            if (search != "")
            {

                var asset = (from a in list.Assets()
                             join e in list.Employees()
                          on a.employeeNumber equals e.employeeNumber
                             select new AssetListViewModel
                             {
                                 assetNumber = a.assetNumber,
                                 serialNumber = a.serialNumber,
                                 manufacturer = a.manufacturer,
                                 catergory = a.catergory,
                                 dateadded = a.dateadded,
                                 warranty = a.warranty,
                                 assetstatus = a.assignstatus,
                                 owner = e.firstName + " " + e.lastName,
                                 assetID = a.assetID,
                                 assigneddate = Convert.ToDateTime(a.assigndate).ToShortDateString()
                             })
                         .Where(x => x.assetNumber.Contains(search.ToUpper()) && x.assetstatus == 1)
                         .ToList();
                return View(asset);
            }
            return View();
        }

        //Technician should be able to view an asset report
        [Authorize(Roles = "Technician")]
        public ActionResult AssetReports(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var asset = context.Assets.Single(a => a.assetID == id);

            if (asset == null)
            {
                return HttpNotFound();
            }
            var result = (from a in list.Assets()
                          join e in list.Employees()
                          on a.employeeNumber equals e.employeeNumber
                          select new AssetReport
                          {
                              serialNumber = a.serialNumber,
                              assetNumber = a.assetNumber,
                              catergory = a.catergory,
                              warranty = a.warranty,
                              manufacturer = a.manufacturer,
                              dateadded = a.dateadded,
                              depreciationcost = (al.depreciationCost(a.dateadded, a.costprice)).ToString("R0.00"),
                              assetstatus = a.assignstatus,
                              costprice = (a.costprice).ToString("R0.00"),
                              owner = e.fullname,
                              assetID = a.assetID,
                              assigneddate = a.assigndate,
                              sellprice = (a.costprice - al.depreciationCost(a.dateadded, a.costprice)).ToString("R0.00")
                          }).SingleOrDefault(c => c.assetID == id && c.assetstatus == 1);
            return View(result);
        }
        [HttpGet]
        public ActionResult EmployeeAssets(string id,int?page)
        {
            ViewBag.EmployeeName = context.Employees.Find(id).fullname;
            //ViewBag.InvoiceId = context.Invoices.Where(i=>i.InvoiceNumber == )
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            else
            {
                int PageSize = 6;
                int PageNumber = (page ?? 1);
                return View(list.Assets().Where(p => p.employeeNumber == id).ToPagedList(PageNumber,PageSize));
            }
        }

        public ActionResult AllExistingAssets(int?page)
        {
            int PageSize = 6;
            int PageNumber = (page ?? 1);
            return View(list.Assets().ToPagedList(PageNumber, PageSize));
   
        }

        public JsonResult GetEmployeesA(string term)
        {
            List<string> employees;
            employees = context.Employees.Where(x => x.employeeNumber.StartsWith(term))
                .Select(n => n.employeeNumber).ToList();
            return Json(employees, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectType()
        {

            Asset asse = new Asset();
            asse.assetID = 00;
            asse.catergory = "...";
            asse.assetNumber = "00";
            asse.assigndate = System.DateTime.Now;
            asse.assignstatus = 0;
            asse.costprice = 0;
            asse.dateadded = System.DateTime.Now;
            asse.employeeNumber = "0";
            asse.warranty = "0";
            asse.serialNumber = "0";
            List<Asset> ll = new List<Asset>();
            ll.Add(asse);
            ViewBag.empassets = ll;
            return View(ll);
        }
        [HttpPost]
        public ActionResult SelectType(string employeenumber)
        {
            List<Asset> lass = context.Assets.ToList().FindAll(x => x.employeeNumber == employeenumber);
            Session["empNo"] = employeenumber;

            Session["Subject"] = " ";
            Session["Body"] = " ";
            if (lass != null)
            {
                ViewBag.empassets = lass;
                return View(lass);
            }
            return View();
        }
    }
}