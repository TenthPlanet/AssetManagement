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
using PagedList.Mvc;
using PagedList;
using System.Data.Entity;
using AssetManagement.Business;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

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

        public EmployeeController(IEmployeeRepository repository)
        {
            this.repository = repository;
        }
        //
        // GET: /Employee/
        [AllowAnonymous]
        public ActionResult Index(int?page)
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
            int PageSize = 6;
            int PageNumber = (page ?? 1);
            return View(query.ToPagedList(PageNumber, PageSize));
        
        }
        //Employee profile
        [AllowAnonymous]
        public ActionResult UserProfile()
        {
            return View(db.Employees.Single(emp => emp.employeeNumber == User.Identity.Name));
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
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName");
            ViewBag.departmentID = new SelectList(db.Departments, "departmentID", "departmentName");
            return View();
        }
        [AllowAnonymous]
        public ActionResult EmployeeSummary(string id)
        {
            return View(db.Employees.Single(emp => emp.employeeNumber == id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "employeeNumber,firstName,lastName,IDNumber,gender,hireDate,position,officeNumber,telephoneNumber,mobileNumber,emailAddress,fileName,fileBytes,departmentID,RoleID")] Employee employee)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    var userrole = repository.FindRoles(employee.RoleID);
                    using (var appcontext = new ApplicationDbContext())
                    {
                        var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(appcontext));
                        var PasswordHash = new PasswordHasher();

                        if (!appcontext.Roles.Any(r => r.Name.Equals(userrole.RoleName)))
                        {
                            var store = new RoleStore<IdentityRole>(appcontext);
                            var manager = new RoleManager<IdentityRole>(store);
                            var role = new IdentityRole { Name = userrole.RoleName };

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
                                UserManager.AddToRole(user.Id, userrole.RoleName);
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
                            UserManager.AddToRole(user.Id, userrole.RoleName);
                        }
                        appcontext.SaveChanges();
                    }
                    Image img = Image.FromFile(Server.MapPath(Url.Content("~/Content/default-placeholder.png")));
                    MemoryStream ms = new MemoryStream();
                    img.Save(ms, ImageFormat.Png);
                    ms.Seek(0, SeekOrigin.Begin);
                    employee.fileName = "default-placeholder.png";
                    employee.fileType = "image/png";
                    employee.fileBytes = new byte[ms.Length];
                    ms.Read(employee.fileBytes, 0, (int)ms.Length);

                    //full name
                    employee.fullname = employee.firstName + " " + employee.lastName;
                    var rolez = db.Roles.Single(e => e.RoleID == employee.RoleID);
                    employee.position = rolez.RoleName;

                    repository.Insert(employee);
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
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", employee.RoleID);
            ViewBag.departmentID = new SelectList(db.Departments, "departmentID", "departmentName", employee.departmentID);
            return View(employee);
        }
        [AllowAnonymous]
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
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName");
            ViewBag.departmentID = new SelectList(db.Departments, "departmentID", "departmentName");
            return View(employee);
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee, HttpPostedFileBase file)
        {
            AssetLogic al = new AssetLogic();

            if (ModelState.IsValid)
            {
                var _employee = db.Employees.Find(employee.employeeNumber);

                if (file != null && file.ContentLength > 0)
                {
                    employee.fileName = System.IO.Path.GetFileName(file.FileName);
                    employee.fileType = file.ContentType;
                    employee.fileBytes = al.ConvertToBytes(file);
                }
                else
                {
                    employee.fileName = _employee.fileName;
                    employee.fileType = _employee.fileType;
                    employee.fileBytes = _employee.fileBytes;
                }
                employee.fullname = employee.firstName + " " + employee.lastName;
                employee.position = db.Roles.Single(e => e.RoleID == employee.RoleID).RoleName;

                db.Entry(_employee).State = EntityState.Detached;
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                if (employee.employeeNumber != User.Identity.Name)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("UserProfile");
            }
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", employee.RoleID);
            ViewBag.departmentID = new SelectList(db.Departments, "departmentID", "departmentName", employee.departmentID);
            return View(employee);
        }


        //Technician should be able to view the employees
        // and asset and asset reports

        public ActionResult AllEmployees(int?page)
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

            int PageSize = 6;
            int PageNumber = (page ?? 1);
            return View(query.ToPagedList(PageNumber, PageSize));
            
        }
        
	}
}