using AssetManagement.Domain.Abstract;
using AssetManagement.Domain.Concrete;
using AssetManagement.Domain.Context;
using AssetManagement.Domain.Entities;
using AssetManagement.Domain.QuickResolver;
using AssetManagement.WebUI.Models;
using AssetManagement.WebUI.ViewModel.Employee;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetManagement.WebUI.ViewModel.Asset;
using AssetManagement.Business;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Core.Objects;

namespace AssetManagement.WebUI.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class EmployeeController : Controller
    {

        public EmployeeController() : this(new EmployeeRepository())
        { }

        private readonly IEmployeeRepository repository;
        private AssetResolver resolver = new AssetResolver();
        private readonly AssetManagementEntities db = new AssetManagementEntities();
        AssetLogic al = new AssetLogic();
        public EmployeeController(IEmployeeRepository repository)
        {
            this.repository = repository;
        }
        //
        // GET: /Employee/
        [AllowAnonymous]
        public ActionResult Index()
        {
            var query = (from depart in repository.Departments()
                         join emps in repository.Employees()
                             on depart.departmentID equals emps.departmentID
                         select new EmployeeViewModel 
                         { 
                             employeeNumber = emps.employeeNumber,
                             fullname = emps.firstName +" "+ emps.lastName,
                             emailAddress = emps.emailAddress,
                             officeNumber = emps.officeNumber,
                             telephoneNumber = emps.telephoneNumber,
                             departmentName = depart.departmentName,
                             position = emps.position
                         })
                         .ToList()
                         .OrderBy(x => x.hireDate);
            return View(query);
        }
        public ViewResult Details(string id)
        {
            var model = (from e in resolver.Employees()   
                         join a in resolver.Assets()
                             on e.employeeNumber equals a.employeeNumber
                             join d in resolver.Departments()
                             on e.departmentID equals d.departmentID
                         select new EmployeeViewModel 
                         { 
                             fullname = e.firstName +" "+ e.lastName,
                             employeeNumber = e.employeeNumber,
                             IDNumber = e.IDNumber,
                             hireDate = e.hireDate,
                             departmentName = d.departmentName,
                             position = e.position,
                             emailAddress = e.emailAddress,
                             
                             
                         }).FirstOrDefault(m => m.employeeNumber.Equals(id));
            return View(model);
        }
        public ActionResult Create()
        {
            //ViewBag.departmentID = new SelectList(db.Departments, "departmentID", "departmentName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeViewModel employeemodel)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    var employee = new Employee
                    {
                        firstName = employeemodel.firstName,
                        lastName = employeemodel.lastName,
                        fullname = employeemodel.firstName +" "+ employeemodel.lastName,
                        employeeNumber = employeemodel.employeeNumber,
                        gender = employeemodel.gender,
                        IDNumber = employeemodel.IDNumber,
                        hireDate = employeemodel.hireDate,
                        mobileNumber = employeemodel.mobileNumber,
                        position = employeemodel.position,
                        emailAddress = employeemodel.emailAddress,
                        officeNumber = employeemodel.officeNumber,
                        telephoneNumber = employeemodel.telephoneNumber

                    };

                    var department = new Department
                    {
                        departmentName = employeemodel.departmentName
                    };

                    using (var appcontext = new ApplicationDbContext())
                    {
                        var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(appcontext));
                        var PasswordHash = new PasswordHasher();

                        if (!appcontext.Roles.Any(r => r.Name.Equals(employee.position)))
                        {
                            var store = new RoleStore<IdentityRole>(appcontext);
                            var manager = new RoleManager<IdentityRole>(store);
                            var role = new IdentityRole { Name = employee.position };

                            manager.Create(role);

                            if (!appcontext.Users.Any(u => u.UserName == employee.employeeNumber))
                            {
                                var user = new ApplicationUser
                                {
                                    UserName = employee.employeeNumber,
                                    Email = employee.emailAddress,
                                    EmailConfirmed = true,
                                    PhoneNumber = employee.mobileNumber,
                                    PhoneNumberConfirmed = true,
                                    PasswordHash = PasswordHash.HashPassword(employee.IDNumber.Substring(0, 6)),
                                };

                                UserManager.Create(user);
                                UserManager.AddToRole(user.Id, employee.position);
                            }
                        }
                        if (!appcontext.Users.Any(u => u.UserName == employee.employeeNumber))
                        {
                            var user = new ApplicationUser
                            {
                                UserName = employee.employeeNumber,
                                Email = employee.emailAddress,
                                EmailConfirmed = true,
                                PhoneNumber = employee.mobileNumber,
                                PhoneNumberConfirmed = true,
                                PasswordHash = PasswordHash.HashPassword(employee.IDNumber.Substring(0, 6)),
                            };

                            UserManager.Create(user);
                            UserManager.AddToRole(user.Id, employee.position);
                        }
                        appcontext.SaveChanges();
                    }
                    repository.Insert(department, employee);
                    repository.Save();
                    TempData["Success"] = employee.firstName + " " + employee.lastName + " has successfully been added!";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ViewBag.Message = "Employee not added. Error: " + e.Message;
                }
              
            }
            ModelState.Clear();
            return View(employeemodel);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);

            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.departmentID = new SelectList(repository.Departments(),"departmentID", "departmentName", employee.departmentID);
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.departmentID = new SelectList(db.Departments, "departmentID", "departmentName", employee.departmentID);
            return View(employee);
        }

        //Technician should be able to view the employees
        // and asset and asset reports

        public ActionResult AllEmployees()
        {
            var query = (from depart in repository.Departments()
                         join emps in repository.Employees()
                             on depart.departmentID equals emps.departmentID
                         select new EmployeeViewModel
                         {
                             employeeNumber = emps.employeeNumber,
                             fullname = emps.firstName + " " + emps.lastName,
                             emailAddress = emps.emailAddress,
                             officeNumber = emps.officeNumber,
                             telephoneNumber = emps.telephoneNumber,
                             departmentName = depart.departmentName,
                             position = emps.position
                         })
                         .ToList()
                         .OrderBy(x => x.hireDate);
            return View(query);
        }
     
        public ActionResult MyAssets(string id)
        {
            Employee ee = db.Employees.Find(id);
            ViewBag.name = ee.fullname.ToString()+"'S     ";

            List<AssetListViewModel> alvmlist = db.Assets.Where(q => q.employeeNumber == id).Select(x => new AssetListViewModel()
            {
                assetNumber = x.assetNumber,
                serialNumber = x.serialNumber,
                catergory = x.catergory,
                dateadded = x.dateadded,
                warranty = x.warranty,
                costprice = "R " + SqlFunctions.StringConvert(x.costprice),
                manufacturer = x.manufacturer,
                assetstatus = x.assignstatus,
                depreciationcost = "R "+SqlFunctions.StringConvert(x.costprice - ((x.costprice * 0.3) * (DateTime.Now.Year - x.dateadded.Year))),
                owner = x.Employee.fullname,
                employeenumber = x.employeeNumber,
                assigneddate = " "+ EntityFunctions.TruncateTime(x.assigndate),
                assetID = x.assetID,
                sell = "R " + SqlFunctions.StringConvert(x.costprice -( x.costprice - ((x.costprice * 0.3) * (DateTime.Now.Year - x.dateadded.Year))))
            }).ToList();

            //foreach (var item in db.Assets.ToList())
            //{

            //    if (item.employeeNumber == id)
            //    {
            //        alvm.assetNumber = item.assetNumber;
            //    alvm.serialNumber = item.serialNumber;
            //    alvm.catergory = item.catergory;
            //    alvm.dateadded = item.dateadded;
            //    alvm.warranty = item.warranty;
            //    alvm.costprice = item.costprice.ToString("R0.00");
            //    alvm.manufacturer = item.manufacturer;
            //    alvm.assetstatus = item.assignstatus;
            //    alvm.depreciationcost = (al.depreciationCost(item.dateadded, item.costprice)).ToString("R0.00");
            //    alvm.owner = item.Employee.fullname;
            //    alvm.employeenumber = item.Employee.employeeNumber;
            //    alvm.assigneddate = Convert.ToDateTime(item.assigndate).ToShortDateString();
            //    alvm.assetID = item.assetID;
            //    alvm.sell = (item.costprice - al.depreciationCost(item.dateadded, item.costprice)).ToString("R0.00");
            //    alvm.department = item.Employee.Departments.departmentName;

            //    alvmlist.Add(alvm);
            //    } 
            //}


            return View(alvmlist);

        }
    }
}